using desktopapplication.Models;
using desktopapplication.ViewModels;

namespace desktopapplication.Views;

public partial class PaymentRecordsPage : ContentPage
{
    private PaymentRecordsViewModel _viewModel;

    public PaymentRecordsPage(Group group, Payment payment)
    {
        InitializeComponent();

        BindingContext = _viewModel = new PaymentRecordsViewModel(group, payment);
    }
}