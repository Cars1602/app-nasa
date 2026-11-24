namespace SpaceExplorer.Vistas;

public partial class InicioPage : ContentPage
{
    public InicioPage()
    {
        InitializeComponent();
    }

    private async void AbrirExploracion(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Explorar");
    }

    private async void AbrirCategoria(object? sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: string tema })
        {
            await Shell.Current.GoToAsync("//Explorar", new ShellNavigationQueryParameters { ["tema"] = tema });
        }
    }

    private async void AbrirColeccion(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Coleccion");
    }
}
