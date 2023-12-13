using desktopapplication.Models;

namespace desktopapplication.ViewModels;

public class GroupsViewModel : BaseViewModel
{
    public GroupsViewModel()
    {
        LoadGroupsCommand = new Command(LoadGroups);
        LoadGroups();
    }

    public Command LoadGroupsCommand { get; }

    public IEnumerable<Group> Groups => Repository.Groups ?? Enumerable.Empty<Group>();

    private void LoadGroups() => Task.Run(async () =>
    {
        await LoadOnTask(Repository.FetchGroups());
        OnPropertyChanged(nameof(Groups));
    });
}