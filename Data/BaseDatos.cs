using System.Diagnostics;
using SpaceExplorer.Modelo;
using SQLite;

namespace SpaceExplorer.Data;

public class BaseDatos
{
    private readonly SQLiteAsyncConnection conexion;
    private readonly SemaphoreSlim bloqueoInicializacion = new(1, 1);
    private bool inicializada;
    private static readonly HttpClient ClienteImagenes = new();
    private readonly HttpClient clienteImagenes;
    public string CarpetaImagenes { get; }

    public BaseDatos(string rutaBaseDatos, HttpClient? clientePruebas = null)
    {
        string carpeta = Path.GetDirectoryName(Path.GetFullPath(rutaBaseDatos))!;
        Directory.CreateDirectory(carpeta);
        CarpetaImagenes = Path.Combine(carpeta, "ImagenesColeccion");
        clienteImagenes = clientePruebas ?? ClienteImagenes;
        conexion = new SQLiteAsyncConnection(rutaBaseDatos,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
    }

    private async Task InicializarAsync()
    {
        if (inicializada) return;
        await bloqueoInicializacion.WaitAsync();
        try
        {
            if (inicializada) return;
            await conexion.CreateTableAsync<DescubrimientoGuardado>();
            inicializada = true;
        }
        finally { bloqueoInicializacion.Release(); }
    }

    public async Task<List<DescubrimientoGuardado>> ConsultarAsync()
    {
        await InicializarAsync();
        var registros = await conexion.Table<DescubrimientoGuardado>().OrderByDescending(x => x.Id).ToListAsync();
        registros.ForEach(CompletarRutaImagen);
        return registros;
    }

    public async Task<DescubrimientoGuardado?> BuscarPorNasaIdAsync(string nasaId)
    {
        await InicializarAsync();
        var registro = await conexion.Table<DescubrimientoGuardado>().Where(x => x.NasaId == nasaId).FirstOrDefaultAsync();
        if (registro != null) CompletarRutaImagen(registro);
        return registro;
    }

    public async Task<(DescubrimientoGuardado Registro, bool Nuevo)> GuardarAsync(
        Descubrimiento descubrimiento, string notas, string categoria)
    {
        if (string.IsNullOrWhiteSpace(descubrimiento.NasaId))
            throw new ArgumentException("El descubrimiento debe tener un identificador NASA.");
        ValidarNotasCategoria(notas, categoria);
        var existente = await BuscarPorNasaIdAsync(descubrimiento.NasaId);
        if (existente != null) return (existente, false);

        var registro = new DescubrimientoGuardado
        {
            NasaId = descubrimiento.NasaId, Titulo = descubrimiento.Titulo,
            Descripcion = descubrimiento.Descripcion,
            FechaCreacionIso = descubrimiento.FechaCreacion?.ToString("O") ?? "",
            Centro = descubrimiento.Centro, TipoContenido = descubrimiento.TipoContenido,
            UrlImagen = descubrimiento.UrlImagen,
            Notas = notas.Trim(), Categoria = categoria.Trim(),
            ArchivoImagen = await DescargarImagenAsync(descubrimiento.UrlImagen)
        };

        try
        {
            // UNIQUE y OR IGNORE evitan duplicados incluso con guardados simultáneos.
            int filas = await conexion.InsertAsync(registro, "OR IGNORE");
            if (filas == 0)
            {
                BorrarCopiaImagen(registro.ArchivoImagen);
                var duplicado = await BuscarPorNasaIdAsync(descubrimiento.NasaId);
                return (duplicado ?? throw new InvalidOperationException("No se pudo recuperar el descubrimiento."), false);
            }
            CompletarRutaImagen(registro);
            return (registro, true);
        }
        catch
        {
            BorrarCopiaImagen(registro.ArchivoImagen);
            throw;
        }
    }

    public async Task ActualizarAsync(int id, string notas, string categoria)
    {
        ValidarNotasCategoria(notas, categoria);
        await InicializarAsync();
        // Se editan las notas y categoría, conservando los metadatos de la NASA.
        int filas = await conexion.ExecuteAsync(
            "UPDATE Descubrimientos SET Notas = ?, Categoria = ? WHERE Id = ?",
            notas.Trim(), categoria.Trim(), id);
        if (filas == 0) throw new InvalidOperationException("El registro ya no existe en la colección.");
    }

    public async Task EliminarAsync(int id)
    {
        await InicializarAsync();
        var registro = await conexion.FindAsync<DescubrimientoGuardado>(id);
        if (registro == null) return;
        await conexion.DeleteAsync(registro);
        BorrarCopiaImagen(registro.ArchivoImagen);
    }

    public Task CerrarAsync() => conexion.CloseAsync();

    private static void ValidarNotasCategoria(string notas, string categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria) || categoria.Trim().Length > 50)
            throw new ArgumentException("Escribe una categoría de entre 1 y 50 caracteres.");
        if (notas.Length > 2000)
            throw new ArgumentException("Las notas no pueden superar los 2000 caracteres.");
    }

    private void CompletarRutaImagen(DescubrimientoGuardado registro)
    {
        registro.RutaImagenLocal = registro.ArchivoImagen == Path.GetFileName(registro.ArchivoImagen)
            && !string.IsNullOrWhiteSpace(registro.ArchivoImagen)
            ? Path.Combine(CarpetaImagenes, registro.ArchivoImagen) : "";
    }

    private void BorrarCopiaImagen(string archivo)
    {
        if (string.IsNullOrWhiteSpace(archivo) || archivo != Path.GetFileName(archivo)) return;
        try { File.Delete(Path.Combine(CarpetaImagenes, archivo)); }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        { Debug.WriteLine("No se pudo retirar una copia local: " + ex.Message); }
    }

    private async Task<string> DescargarImagenAsync(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || uri.Scheme != Uri.UriSchemeHttps) return "";
        string extension = Path.GetExtension(uri.AbsolutePath).ToLowerInvariant();
        if (extension is not (".jpg" or ".jpeg" or ".png" or ".webp")) return "";
        string archivo = "";
        try
        {
            using var cancelacion = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            using var respuesta = await clienteImagenes.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancelacion.Token);
            respuesta.EnsureSuccessStatusCode();
            const int maximoBytes = 5 * 1024 * 1024;
            if (respuesta.Content.Headers.ContentLength > maximoBytes
                || respuesta.RequestMessage?.RequestUri?.Scheme is not (null or "https")) return "";
            string? tipo = respuesta.Content.Headers.ContentType?.MediaType;
            if (tipo is not ("image/jpeg" or "image/png" or "image/webp")) return "";

            Directory.CreateDirectory(CarpetaImagenes);
            archivo = Guid.NewGuid().ToString("N") + extension;
            await using var entrada = await respuesta.Content.ReadAsStreamAsync(cancelacion.Token);
            await using var salida = File.Create(Path.Combine(CarpetaImagenes, archivo));
            byte[] buffer = new byte[81920];
            int total = 0;
            int leidos;
            while ((leidos = await entrada.ReadAsync(buffer, cancelacion.Token)) > 0)
            {
                total += leidos;
                if (total > maximoBytes) throw new IOException("La imagen supera el límite de descarga.");
                await salida.WriteAsync(buffer.AsMemory(0, leidos), cancelacion.Token);
            }
            if (total == 0) throw new IOException("La imagen está vacía.");
            return archivo;
        }
        catch (Exception ex) when (ex is HttpRequestException or IOException or OperationCanceledException or UnauthorizedAccessException)
        {
            BorrarCopiaImagen(archivo);
            Debug.WriteLine("Se guardará el texto sin imagen local: " + ex.Message);
            return "";
        }
    }
}
