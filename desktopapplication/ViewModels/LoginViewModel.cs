using desktopapplication.services.api;

namespace desktopapplication.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private string? _message;
    private string _password = string.Empty;
    private string _username = string.Empty;

    public LoginViewModel() => LoginCommand = new Command(Login);

    public Command LoginCommand { get; }

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

    private async void Login()
    {
        Message = null;
        try
        {
            await LoadOnTask(Repository.Login(Username, Password));

            //go to home page after login
            await Shell.Current.Navigation.PopToRootAsync();
        }
        catch (WrongLoginCredentialsException e)
        {
            Message = e.Message;
        }
    }
}