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
        Payment = await LoadOnTask(Repository.GetPayment(Payment.GroupId, Payment.Id)));
}