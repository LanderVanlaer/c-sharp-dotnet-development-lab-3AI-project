using desktopapplication.services.api;

namespace desktopapplication.ViewModels;

public class RegisterViewModel : BaseViewModel
{
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

    private async void Register()
    {
        if (Password != PasswordConfirmation)
        {
            await DisplayAlert("Error", "Passwords do not match", "Ok");
            return;
        }

        try
        {
            await LoadOnTask(Repository.Register(Username, Password));
            Username = Password = PasswordConfirmation = String.Empty;

            await DisplayAlert("Error", "Successfully registered", "Ok");
        }
        catch (UserNameAlreadyExistsException e)
        {
            await DisplayAlert("Error", e.Message, "Ok");
        }
        catch (ApiError e)
        {
            await DisplayAlert(
                "Error",
                e.Body?.Errors != null && e.Body.Errors.Count != 0 ? string.Join("\n", e.Body.Errors) : e.Message,
                "Ok");
        }
    }
}