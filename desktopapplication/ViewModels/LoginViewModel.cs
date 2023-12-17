using desktopapplication.services.api;

namespace desktopapplication.ViewModels;

public class LoginViewModel : BaseViewModel
{
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

    private async void Login()
    {
        try
        {
            await LoadOnTask(Repository.Login(Username, Password));

            Username = string.Empty;
            Password = string.Empty;

            //go to home page after login
            await Shell.Current.Navigation.PopToRootAsync();
        }
        catch (WrongLoginCredentialsException e)
        {
            await DisplayAlert("Login failed", e.Message, "Try again");
        }
    }
}