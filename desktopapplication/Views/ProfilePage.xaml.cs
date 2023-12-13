using desktopapplication.ViewModels;

namespace desktopapplication.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfilePageViewModel _viewModel;

    public ProfilePage()
    {
        InitializeComponent();

        BindingContext = _viewModel = new ProfilePageViewModel();
    }
}