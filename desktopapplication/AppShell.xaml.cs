using System.Diagnostics;
using desktopapplication.services;

namespace desktopapplication;

public partial class AppShell : Shell
{
    private readonly IRepository _repository = DependencyService.Get<IRepository>();

    public AppShell()
    {
        InitializeComponent();

        _repository.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName != null && args.PropertyName != nameof(IRepository.IsAuthenticated)) return;
            Debug.WriteLine("User " + (IsAuthenticated ? "is" : "not") + " authenticated");

            OnPropertyChanged(nameof(IsAuthenticated));
            OnPropertyChanged(nameof(IsNotAuthenticated));


            // Not doing the following check will cause all the waiting tasks to be removed from the queue
            FlyoutItem newItem = IsAuthenticated ? HomeFlyoutItem : LoginFlyoutItem;
            if (CurrentItem == newItem) return;
            CurrentItem = newItem;
        };

        CurrentItem = IsAuthenticated ? HomeFlyoutItem : LoginFlyoutItem;

        BindingContext = this;
    }

    public bool IsAuthenticated => _repository.IsAuthenticated;
    public bool IsNotAuthenticated => !_repository.IsAuthenticated;
}