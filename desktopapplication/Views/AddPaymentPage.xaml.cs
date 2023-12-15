using desktopapplication.Models;
using desktopapplication.ViewModels;

namespace desktopapplication.Views;

public partial class AddPaymentPage : ContentPage
{
    private readonly AddPaymentPageViewModel _viewModel;
    private readonly Group _group;

    public AddPaymentPage(Group group)
    {
        InitializeComponent();
        _group = group;

        BindingContext = _viewModel = new AddPaymentPageViewModel(_group);
    }
}