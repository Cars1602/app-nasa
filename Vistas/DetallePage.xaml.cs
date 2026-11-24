using System.Net;
using System.Text.RegularExpressions;
using SpaceExplorer.Modelo;

namespace SpaceExplorer.Vistas;

public partial class DetallePage : ContentPage
{
    private readonly Descubrimiento descubrimiento;
    private DescubrimientoGuardado? registroActual;
    private bool guardando;

    public DetallePage(Descubrimiento descubrimiento, DescubrimientoGuardado? guardado = null)
    {
        InitializeComponent();
        this.descubrimiento = descubrimiento;
        registroActual = guardado;
        if (guardado != null)
            VolverButton.Text = "Volver a mi colección";
        ImagenAmpliada.Source = guardado?.ImagenMostrar ?? descubrimiento.UrlImagen;
        BindingContext = descubrimiento;
        FechaLabel.Text = descubrimiento.FechaCreacion.HasValue
            ? $"Fecha de creación: {descubrimiento.FechaCreacion.Value:dd/MM/yyyy}"
            : "Fecha de creación no disponible";
        // Algunos textos contienen etiquetas HTML: mostrar texto, sin ejecutar HTML.
        DescripcionLabel.Text = WebUtility.HtmlDecode(Regex.Replace(descubrimiento.Descripcion, "<[^>]*>", " "));
        bool tieneImagen = guardado != null || !string.IsNullOrWhiteSpace(descubrimiento.UrlImagen);
        ImagenAmpliada.IsVisible = tieneImagen;
        SinImagenLabel.IsVisible = !tieneImagen;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            registroActual = await App.BaseDatos.BuscarPorNasaIdAsync(descubrimiento.NasaId);
            if (registroActual != null)
            {
                MostrarRegistroGuardado();
                EstadoColeccionLabel.Text = registroActual.EstadoImagen;
            }
            else
                EstadoColeccionLabel.Text = "Añade notas y guarda este descubrimiento para consultarlo sin conexión.";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            EstadoColeccionLabel.Text = "No se pudo consultar la colección. Puedes volver a intentar guardar.";
        }
        finally { GuardarButton.IsEnabled = true; }
    }

    private void MostrarRegistroGuardado()
    {
        if (registroActual == null) return;
        CategoriaEntry.Text = registroActual.Categoria;
        NotasEditor.Text = registroActual.Notas;
        GuardarButton.Text = "Actualizar notas y categoría";
        // Al abrir un guardado se usa exclusivamente su imagen local, sin consultar la API.
        ImagenAmpliada.Source = registroActual.ImagenMostrar;
        ImagenAmpliada.IsVisible = true;
        SinImagenLabel.IsVisible = false;
    }

    private async void GuardarColeccion(object? sender, EventArgs e)
    {
        if (guardando) return;
        guardando = true;
        GuardarButton.IsEnabled = false;
        CategoriaEntry.IsEnabled = false;
        NotasEditor.IsEnabled = false;
        try
        {
            string notas = NotasEditor.Text ?? "";
            string categoria = CategoriaEntry.Text ?? "";
            if (registroActual == null)
            {
                EstadoColeccionLabel.Text = "Guardando descubrimiento y preparando copia de la imagen…";
                var resultado = await App.BaseDatos.GuardarAsync(descubrimiento, notas, categoria);
                registroActual = resultado.Registro;
                MostrarRegistroGuardado();
                EstadoColeccionLabel.Text = !resultado.Nuevo
                    ? "Ya estaba en tu colección. Se conservaron tus notas anteriores."
                    : File.Exists(registroActual.RutaImagenLocal)
                        ? "Descubrimiento e imagen guardados para consultar sin conexión."
                        : "Texto y notas guardados. No se pudo descargar la imagen para uso sin conexión.";
            }
            else
            {
                await App.BaseDatos.ActualizarAsync(registroActual.Id, notas, categoria);
                EstadoColeccionLabel.Text = "Notas y categoría actualizadas.";
            }
        }
        catch (ArgumentException ex) { EstadoColeccionLabel.Text = ex.Message; }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            EstadoColeccionLabel.Text = "No se pudo guardar. Revisa el espacio disponible y vuelve a intentarlo.";
        }
        finally
        {
            guardando = false;
            GuardarButton.IsEnabled = true;
            CategoriaEntry.IsEnabled = true;
            NotasEditor.IsEnabled = true;
        }
    }

    private async void VolverResultados(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
