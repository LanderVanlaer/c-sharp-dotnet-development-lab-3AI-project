using desktopapplication.Models;

namespace desktopapplication.ViewModels;

public class PaymentRecordsViewModel : BaseViewModel
{
    private Payment _payment;

    public PaymentRecordsViewModel(Group group, Payment payment)
    {
        Group = group;
        _payment = payment;

        LoadPaymentRecordsCommand = new Command(LoadPaymentRecords);
        LoadPaymentRecords();
    }

    public string Title => "Payment " + Payment.Name;
    public Group Group { get; }

    public Command LoadPaymentRecordsCommand { get; }

    public Payment Payment
    {
        get => _payment;
        private set => SetField(ref _payment, value);
    }

    private void LoadPaymentRecords() => Task.Run(async () =>
    {
        StartLoading();

        Task<ICollection<User>> fetchUsers = Repository.FetchUsers(Payment.GroupId);
        Task<Payment> getPayment = Repository.GetPayment(Payment.GroupId, Payment.Id);

        await Task.WhenAll(fetchUsers, getPayment);

        Payment = getPayment.Result;

        StopLoading();
    });

    protected override void OnAuthenticated() => LoadPaymentRecords();
}