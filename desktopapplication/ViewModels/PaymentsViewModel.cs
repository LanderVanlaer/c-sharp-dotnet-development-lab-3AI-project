using desktopapplication.Models;

namespace desktopapplication.ViewModels;

public class PaymentsViewModel : BaseViewModel
{
    public PaymentsViewModel(Group group)
    {
        Group = group;
        LoadPaymentsCommand = new Command(LoadPayments);
        LoadPayments();

        Repository.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(Repository.Payments)) OnPropertyChanged(nameof(Payments));
        };
    }

    public Group Group { get; }
    public string Title => "Payments of " + Group.Name;

    public Command LoadPaymentsCommand { get; }

    public IEnumerable<Payment> Payments =>
        (Repository.Payments ?? Enumerable.Empty<Payment>()).OrderByDescending(payment => payment.CreatedAt);

    private void LoadPayments() => Task.Run(async () =>
    {
        await LoadOnTask(Task.WhenAll(
            Repository.FetchPayments(Group.Id),
            Repository.FetchUsers(Group.Id)
        ));

        OnPropertyChanged(nameof(Payments));
    });
}