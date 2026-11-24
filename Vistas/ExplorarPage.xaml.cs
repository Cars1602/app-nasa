using System.Diagnostics;
using System.Net;
using System.Text.Json;
using SpaceExplorer.Data;
using SpaceExplorer.Modelo;

namespace SpaceExplorer.Vistas;

public partial class ExplorarPage : ContentPage, IQueryAttributable
{
    private readonly ApiServicio apiServicio = new();
    private CancellationTokenSource? cancelacionBusqueda;
    private bool buscando;
    private bool abriendoDetalle;
    private string? temaPendiente;

    public ExplorarPage()
    {
        InitializeComponent();
    }

    private async void BuscarImagenes(object? sender, EventArgs e)
    {
        await RealizarBusqueda();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> parametros)
    {
        if (parametros.TryGetValue("tema", out var valor) && valor is string tema)
        {
            BusquedaEntry.Text = tema;
            temaPendiente = tema;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (temaPendiente != null && !buscando)
        {
            temaPendiente = null;
            await RealizarBusqueda();
        }
    }

    private async void BuscarCategoria(object? sender, EventArgs e)
    {
        if (!buscando && sender is Button { CommandParameter: string tema })
        {
            BusquedaEntry.Text = tema;
            await RealizarBusqueda();
        }
    }

    private async void AbrirDetalle(object? sender, SelectionChangedEventArgs e)
    {
        if (abriendoDetalle || e.CurrentSelection.FirstOrDefault() is not Descubrimiento descubrimiento)
            return;

        abriendoDetalle = true;
        ResultadosLista.SelectedItem = null;
        try
        {
            await Navigation.PushAsync(new DetallePage(descubrimiento));
        }
        finally
        {
            abriendoDetalle = false;
        }
    }

    private async Task RealizarBusqueda()
    {
        if (buscando)
            return;

        string texto = BusquedaEntry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(texto))
        {
            EstadoLabel.Text = "Escribe un tema antes de buscar.";
            return;
        }

        buscando = true;
        BuscarButton.IsEnabled = false;
        TemasPanel.IsEnabled = false;
        BusquedaEntry.IsEnabled = false;
        CargaIndicator.IsVisible = true;
        CargaIndicator.IsRunning = true;
        ResultadosLista.ItemsSource = null;
        EstadoLabel.Text = "Consultando la biblioteca de la NASA…";
        var cancelacionActual = new CancellationTokenSource();
        cancelacionBusqueda = cancelacionActual;

        try
        {
            var resultados = await apiServicio.BuscarImagenes(texto, cancelacionActual.Token);
            ResultadosLista.ItemsSource = resultados;
            EstadoLabel.Text = resultados.Count == 0
                ? "No encontramos imágenes. Prueba otro término, preferentemente en inglés."
                : $"Se muestran {resultados.Count} imágenes. Fuente: NASA Image and Video Library.";
        }
        catch (OperationCanceledException)
        {
            EstadoLabel.Text = cancelacionActual.IsCancellationRequested
                ? "Búsqueda cancelada."
                : "La consulta tardó demasiado. Inténtalo nuevamente.";
        }
        catch (HttpRequestException ex)
        {
            EstadoLabel.Text = ex.StatusCode == HttpStatusCode.TooManyRequests
                ? "La NASA recibió demasiadas consultas. Espera un momento y vuelve a intentar."
                : "No se pudo consultar la NASA. Revisa tu conexión e inténtalo nuevamente.";
        }
        catch (JsonException)
        {
            EstadoLabel.Text = "La NASA devolvió datos que no pudimos interpretar. Inténtalo más tarde.";
        }
        catch (ArgumentException ex)
        {
            EstadoLabel.Text = ex.Message;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            EstadoLabel.Text = "No pudimos completar la búsqueda. Vuelve a intentarlo.";
        }
        finally
        {
            buscando = false;
            BuscarButton.IsEnabled = true;
            TemasPanel.IsEnabled = true;
            BusquedaEntry.IsEnabled = true;
            CargaIndicator.IsRunning = false;
            CargaIndicator.IsVisible = false;
            cancelacionBusqueda = null;
            cancelacionActual.Dispose();
        }
    }

    protected override void OnDisappearing()
    {
        cancelacionBusqueda?.Cancel();
        base.OnDisappearing();
    }
}
