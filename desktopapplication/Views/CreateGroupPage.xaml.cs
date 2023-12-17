using desktopapplication.ViewModels;

namespace desktopapplication.Views;

public partial class CreateGroupPage : ContentPage
{
    private CreateGroupPageViewModel _viewModel;

    public CreateGroupPage()
    {
        InitializeComponent();

        BindingContext = _viewModel = new CreateGroupPageViewModel();
    }
}