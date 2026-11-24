using System.Globalization;
using System.Text.Json;
using SpaceExplorer.Modelo;

namespace SpaceExplorer.Data;

public class ApiServicio
{
    // Se reutiliza la conexión entre búsquedas, en lugar de crear un cliente por petición.
    private static readonly HttpClient ClienteCompartido = new()
    {
        Timeout = TimeSpan.FromSeconds(25)
    };
    private readonly HttpClient cliente;

    public ApiServicio(HttpClient? clientePruebas = null)
    {
        cliente = clientePruebas ?? ClienteCompartido;
    }

    public async Task<List<Descubrimiento>> BuscarImagenes(string texto,
        CancellationToken cancelacion = default)
    {
        if (string.IsNullOrWhiteSpace(texto) || texto.Trim().Length > 120)
            throw new ArgumentException("Escribe un tema de entre 1 y 120 caracteres.");

        string consulta = Uri.EscapeDataString(texto.Trim());
        string url = $"https://images-api.nasa.gov/search?q={consulta}&media_type=image&page_size=10";

        using HttpResponseMessage respuesta = await cliente.GetAsync(url, cancelacion);
        respuesta.EnsureSuccessStatusCode();
        using Stream contenido = await respuesta.Content.ReadAsStreamAsync(cancelacion);
        NasaResponse? datos = await JsonSerializer.DeserializeAsync(contenido,
            NasaJsonContext.Default.NasaResponse, cancelacion);

        if (datos?.Coleccion == null)
            throw new JsonException("La respuesta no contiene una colección NASA válida.");

        var descubrimientos = new List<Descubrimiento>();
        foreach (NasaRecurso? recurso in datos.Coleccion.Recursos ?? [])
        {
            NasaDatos? informacion = recurso?.Datos?.FirstOrDefault(d => d?.TipoContenido == "image");
            if (informacion == null || string.IsNullOrWhiteSpace(informacion.NasaId))
                continue;

            // Preferir miniaturas: los originales pueden ser TIFF de decenas de MB.
            NasaEnlace? imagen = recurso?.Enlaces?.FirstOrDefault(e =>
                e?.Relacion == "preview" && EsImagenWeb(e));
            imagen ??= recurso?.Enlaces?.FirstOrDefault(EsImagenWeb);

            DateTimeOffset? fecha = DateTimeOffset.TryParse(informacion.FechaCreacion,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var valorFecha)
                ? valorFecha : null;

            descubrimientos.Add(new Descubrimiento
            {
                NasaId = informacion.NasaId,
                Titulo = string.IsNullOrWhiteSpace(informacion.Titulo) ? "Sin título" : informacion.Titulo,
                Descripcion = string.IsNullOrWhiteSpace(informacion.Descripcion)
                    ? "Descripción no disponible." : informacion.Descripcion,
                FechaCreacion = fecha,
                Centro = string.IsNullOrWhiteSpace(informacion.Centro) ? "Centro no indicado" : informacion.Centro,
                TipoContenido = informacion.TipoContenido ?? "image",
                UrlImagen = imagen?.Url ?? ""
            });
        }

        return descubrimientos;
    }

    private static bool EsImagenWeb(NasaEnlace? enlace)
    {
        if (enlace?.Tipo != "image" || !Uri.TryCreate(enlace.Url, UriKind.Absolute, out var uri)
            || uri.Scheme != Uri.UriSchemeHttps)
            return false;

        string extension = Path.GetExtension(uri.AbsolutePath).ToLowerInvariant();
        return extension is ".jpg" or ".jpeg" or ".png" or ".webp";
    }
}
