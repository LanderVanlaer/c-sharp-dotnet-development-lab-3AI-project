using desktopapplication.Models;
using desktopapplication.services;
using desktopapplication.services.api;

namespace desktopapplication.ViewModels;

public class ProfileEditPageViewModel : BaseViewModel
{
    #region Private Fields

    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _passwordConfirm = string.Empty;

    private string? _message;
    private string? _okMessage;

    #endregion

    #region Public Fields

    public DateTime CreatedAt => User.CreatedAt;
    public Guid Id => User.Id;

    public string Username
    {
        get => _username;
        set => SetField(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }

    public string PasswordConfirm
    {
        get => _passwordConfirm;
        set => SetField(ref _passwordConfirm, value);
    }

    public string? Message
    {
        get => _message;
        set
        {
            SetField(ref _message, value);
            OnPropertyChanged(nameof(HasMessage));
        }
    }

    public bool HasMessage => Message != null;

    public string? OkMessage
    {
        get => _okMessage;
        set
        {
            SetField(ref _okMessage, value);
            OnPropertyChanged(nameof(HasOkMessage));
        }
    }

    public bool HasOkMessage => OkMessage != null;

    #endregion

    public ProfileEditPageViewModel()
    {
        LoadUserCommand = new Command(LoadUser);
        UpdateUserCommand = new Command(UpdateUser);
        LoadUser();

        Repository.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName != null && args.PropertyName != nameof(IRepository.User)) return;
            OnPropertyChanged(nameof(User));

            OnPropertyChanged(nameof(CreatedAt));
            OnPropertyChanged(nameof(Id));

            if (Username == String.Empty)
                Username = User.Username;
        };
    }

    public Command LoadUserCommand { get; }
    public Command UpdateUserCommand { get; }

    public User User => Repository.User ?? new User();

    private void LoadUser() => Task.Run(async () =>
    {
        await LoadOnTask(Repository.FetchUser());
        OnPropertyChanged(nameof(User));
    });

    private void UpdateUser() => Task.Run(async () =>
    {
        Message = OkMessage = null;

        if (Password != PasswordConfirm)
        {
            Message = "Passwords do not match";
            return;
        }

        try
        {
            await LoadOnTask(Repository.UpdateUser(Username, Password == String.Empty ? null : Password));

            OkMessage = "Successfully registered";
        }
        catch (UserNameAlreadyExistsException e)
        {
            Message = e.Message;
        }
        catch (ApiError e)
        {
            Message = e.Body?.Errors != null && e.Body.Errors.Count != 0 ? string.Join("\n", e.Body.Errors) : e.Message;
        }
    });
}