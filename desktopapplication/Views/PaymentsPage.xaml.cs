using desktopapplication.Models;
using desktopapplication.ViewModels;

namespace desktopapplication.Views;

public partial class PaymentsPage : ContentPage
{
    private PaymentsViewModel _viewModel;

    public PaymentsPage(Group group)
    {
        InitializeComponent();

        BindingContext = _viewModel = new PaymentsViewModel(group);
    }

    private void PaymentsListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        throw new NotImplementedException();
    }
}