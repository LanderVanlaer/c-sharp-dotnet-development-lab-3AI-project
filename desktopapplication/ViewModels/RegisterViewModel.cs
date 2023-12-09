using desktopapplication.services.api;

namespace desktopapplication.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    private string? _message;
    private string? _okMessage;
    private string _password = string.Empty;
    private string _passwordConfirmation = string.Empty;
    private string _username = string.Empty;

    public RegisterViewModel() => RegisterCommand = new Command(Register);

    public Command RegisterCommand { get; }

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

    public string PasswordConfirmation
    {
        get => _passwordConfirmation;
        set => SetField(ref _passwordConfirmation, value);
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

    private void Register() => Task.Run(async () =>
    {
        Message = null;
        OkMessage = null;

        if (Password != PasswordConfirmation)
        {
            Message = "Passwords do not match";
            return;
        }

        try
        {
            await LoadOnTask(Repository.Register(Username, Password));
            Username = Password = PasswordConfirmation = String.Empty;

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