using Microsoft.Extensions.DependencyInjection;

namespace SpaceExplorer;

public partial class App : Application
{
	private static Data.BaseDatos? baseDatos;
	public static Data.BaseDatos BaseDatos => baseDatos ??= new Data.BaseDatos(
		Path.Combine(FileSystem.AppDataDirectory, "SpaceExplorer.db3"));

	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}
