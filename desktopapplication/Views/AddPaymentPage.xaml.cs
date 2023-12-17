using desktopapplication.Models;
using desktopapplication.ViewModels;

namespace desktopapplication.Views;

public partial class AddPaymentPage : ContentPage
{
    private readonly Group _group;
    private readonly AddPaymentPageViewModel _viewModel;

    public AddPaymentPage(Group group)
    {
        InitializeComponent();
        _group = group;

        BindingContext = _viewModel = new AddPaymentPageViewModel(_group);
    }
}