using desktopapplication.ViewModels;

namespace desktopapplication.Views;

public partial class RegisterPage : ContentPage
{
    private RegisterViewModel _viewModel;

    public RegisterPage()
    {
        InitializeComponent();

        BindingContext = _viewModel = new RegisterViewModel();
    }
}