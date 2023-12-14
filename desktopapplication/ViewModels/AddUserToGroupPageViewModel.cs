using desktopapplication.Models;
using desktopapplication.services.api;

namespace desktopapplication.ViewModels;

public class AddUserToGroupPageViewModel : BaseViewModel
{
    private readonly Group _group;

    public AddUserToGroupPageViewModel(Group group)
    {
        _group = group;
        Username = string.Empty;

        AddUserToGroupCommand = new Command(AddUserToGroup);
    }

    public string Username { get; set; }

    public Command AddUserToGroupCommand { get; }

    public string Title => "Add user to " + _group.Name;

    public async void AddUserToGroup()
    {
        if (Username.Trim() == String.Empty)
        {
            if (Application.Current?.MainPage is not null)
                await Application.Current.MainPage.DisplayAlert("Error", "Username cannot be empty", "OK");
            return;
        }

        try
        {
            await Repository.AddUserToGroup(_group.Id, Username);
        }
        catch (UserNotFoundException e)
        {
            if (Application.Current?.MainPage is not null)
                await Application.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
            return;
        }
        catch (ApiError e)
        {
            if (Application.Current?.MainPage is not null)
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    e.Body?.Title != null ? e.Body.Title : e.Message,
                    "OK"
                );
            return;
        }

        Task tasks = Task.WhenAll(
            Repository.FetchPayments(_group.Id),
            Repository.FetchUsers(_group.Id)
        );

        if (Application.Current?.MainPage is not null)
            await Application.Current.MainPage.DisplayAlert("", "User has been added to the group", "OK");

        await tasks;
        await Shell.Current.GoToAsync("..");
    }
}