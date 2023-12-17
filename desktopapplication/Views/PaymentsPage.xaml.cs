using desktopapplication.Models;
using desktopapplication.ViewModels;

namespace desktopapplication.Views;

public partial class PaymentsPage : ContentPage
{
    private PaymentsViewModel _viewModel;

    public PaymentsPage(Group group)
    {
        InitializeComponent();
        Group = group;

        BindingContext = _viewModel = new PaymentsViewModel(group);
    }

    private Group Group { get; }

    private async void PaymentsListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Payment payment)
            await Navigation.PushAsync(new PaymentRecordsPage(Group, payment));

        PaymentsListView.SelectedItem = null;
    }

    private async void AddUser_OnClicked(object sender, EventArgs e) =>
        await Navigation.PushAsync(new AddUserToGroupPage(Group));

    private async void AddPayment_OnClicked(object sender, EventArgs e) =>
        await Navigation.PushAsync(new AddPaymentPage(Group));

    private async void OpenLeaderboard_OnClicked(object sender, EventArgs e) =>
        await Navigation.PushAsync(new LeaderboardPage(Group));
}