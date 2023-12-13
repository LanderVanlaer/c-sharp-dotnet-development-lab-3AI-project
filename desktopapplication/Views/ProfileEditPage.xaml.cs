using desktopapplication.ViewModels;

namespace desktopapplication.Views;

public partial class ProfileEditPage : ContentPage
{
    private readonly ProfileEditPageViewModel _viewModel;

    public ProfileEditPage()
    {
        InitializeComponent();

        BindingContext = _viewModel = new ProfileEditPageViewModel();
    }
}