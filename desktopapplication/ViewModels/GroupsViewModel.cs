using System.Diagnostics;
using desktopapplication.Models;

namespace desktopapplication.ViewModels;

public class GroupsViewModel : BaseViewModel
{
    public GroupsViewModel()
    {
        LoadGroupsCommand = new Command(LoadGroups);
        LoadGroups();

        Repository.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(Repository.Groups)) OnPropertyChanged(nameof(Groups));
        };
    }

    public Command LoadGroupsCommand { get; }

    public IEnumerable<Group> Groups => Repository.Groups ?? Enumerable.Empty<Group>();

    private async void LoadGroups()
    {
        await LoadOnTask(Repository.FetchGroups());
    }

    protected override void OnAuthenticated() => LoadGroups();
}