using SpaceExplorer.Modelo;

namespace SpaceExplorer.Vistas;

public partial class ColeccionPage : ContentPage
{
    private bool operando;

    public ColeccionPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarColeccion();
    }

    private async Task CargarColeccion()
    {
        CargaIndicator.IsVisible = true;
        CargaIndicator.IsRunning = true;
        ColeccionLista.IsEnabled = false;
        try
        {
            var registros = await App.BaseDatos.ConsultarAsync();
            ColeccionLista.ItemsSource = registros;
            EstadoLabel.Text = registros.Count == 0 ? "Todavía no guardaste descubrimientos."
                : registros.Count == 1 ? "1 descubrimiento guardado en este dispositivo."
                : $"{registros.Count} descubrimientos guardados en este dispositivo.";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            EstadoLabel.Text = "No se pudo leer la colección. Vuelve a abrir esta pestaña para reintentar.";
        }
        finally
        {
            CargaIndicator.IsVisible = false;
            CargaIndicator.IsRunning = false;
            ColeccionLista.IsEnabled = true;
        }
    }

    private async void AbrirGuardado(object? sender, EventArgs e)
    {
        if (operando || sender is not Button { CommandParameter: DescubrimientoGuardado registro }) return;
        operando = true;
        try
        {
            await Navigation.PushAsync(new DetallePage(registro.ComoDescubrimiento(), registro));
        }
        finally { operando = false; }
    }

    private async void EliminarGuardado(object? sender, EventArgs e)
    {
        if (operando || sender is not Button { CommandParameter: DescubrimientoGuardado registro }) return;
        operando = true;
        try
        {
            bool confirmar = await DisplayAlertAsync("Eliminar descubrimiento",
                $"¿Eliminar «{registro.Titulo}» de tu colección? Se eliminarán sus notas y su copia local de imagen.",
                "Eliminar", "Cancelar");
            if (!confirmar) return;
            await App.BaseDatos.EliminarAsync(registro.Id);
            await CargarColeccion();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            EstadoLabel.Text = "No se pudo eliminar el descubrimiento. Vuelve a intentarlo.";
        }
        finally { operando = false; }
    }

    private async void AbrirExploracion(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Explorar");
    }
}
