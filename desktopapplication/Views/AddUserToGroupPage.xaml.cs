using desktopapplication.Models;
using desktopapplication.ViewModels;

namespace desktopapplication.Views;

public partial class AddUserToGroupPage : ContentPage
{
    private readonly AddUserToGroupPageViewModel _viewModel;
    private readonly Group Group;

    public AddUserToGroupPage(Group group)
    {
        InitializeComponent();

        Group = group;

        BindingContext = _viewModel = new AddUserToGroupPageViewModel(group);
    }
}