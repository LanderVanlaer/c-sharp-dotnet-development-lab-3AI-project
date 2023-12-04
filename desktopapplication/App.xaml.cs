using desktopapplication.services;
using desktopapplication.services.api;

namespace desktopapplication;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        DependencyService.Register<IRepository, RestApiService>();
        MainPage = new AppShell();
    }
}