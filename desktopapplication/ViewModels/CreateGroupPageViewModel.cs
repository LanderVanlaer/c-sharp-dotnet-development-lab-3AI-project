using desktopapplication.services.api;

namespace desktopapplication.ViewModels;

public class CreateGroupPageViewModel : BaseViewModel
{
    private string _name = string.Empty;

    public CreateGroupPageViewModel() => CreateGroupCommand = new Command(CreateGroup);

    public Command CreateGroupCommand { get; }

    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    private async void CreateGroup()
    {
        try
        {
            await LoadOnTask(Repository.CreateGroup(Name));
            await Shell.Current.GoToAsync("..");
        }
        catch (ApiError e)
        {
            await DisplayAlert("Error",
                e.Body?.Errors != null && e.Body.Errors.Count != 0 ? string.Join("\n", e.Body.Errors) : e.Message,
                "OK"
            );
        }
    }
}