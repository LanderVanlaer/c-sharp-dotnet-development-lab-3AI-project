using desktopapplication.Models;

namespace desktopapplication.ViewModels;

public class PaymentsViewModel : BaseViewModel
{
    public PaymentsViewModel(Group group)
    {
        Group = group;
        LoadPaymentsCommand = new Command(LoadPayments);
        LoadPayments();
    }

    public Group Group { get; }
    public string Title => "Payments of " + Group.Name;

    public Command LoadPaymentsCommand { get; }

    public IEnumerable<Payment> Payments => Repository.Payments() ?? Enumerable.Empty<Payment>();

    private void LoadPayments() => Task.Run(async () =>
    {
        await LoadOnTask(Repository.FetchPayments(Group.Id));
        OnPropertyChanged(nameof(Payments));
    });
}