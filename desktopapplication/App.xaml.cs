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

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window = base.CreateWindow(activationState);

        window.Width = Constants.App.Window.Width;
        window.Height = Constants.App.Window.Height;

        return window;
    }
}