using System.Windows;
using GameDashboard.Services;
using GameDashboard.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace GameDashboard;

public partial class App : Application
{
    private readonly ServiceProvider _provider;

    public App()
    {
        var services = new ServiceCollection();

        services.AddHttpClient<JogoService>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<MainWindow>();

        _provider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var window = _provider.GetRequiredService<MainWindow>();
        window.Show();
    }
}