using desktopapplication.Models;
using desktopapplication.services;

namespace desktopapplication.ViewModels;

public class ProfilePageViewModel : BaseViewModel
{
    public ProfilePageViewModel()
    {
        LoadUserCommand = new Command(LoadUser);
        LoadUser();

        Repository.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName != null && args.PropertyName != nameof(IRepository.User)) return;
            OnPropertyChanged(nameof(User));

            OnPropertyChanged(nameof(CreatedAt));
            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(Username));
        };
    }

    public DateTime CreatedAt => User.CreatedAt;
    public Guid Id => User.Id;
    public string Username => User.Username;

    public Command LoadUserCommand { get; }

    public User User => Repository.User ?? new User();

    private void LoadUser() => Task.Run(async () =>
    {
        await LoadOnTask(Repository.FetchUser());
        OnPropertyChanged(nameof(User));
    });
}