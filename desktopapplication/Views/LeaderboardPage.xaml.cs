using desktopapplication.Models;
using desktopapplication.ViewModels;

namespace desktopapplication.Views;

public partial class LeaderboardPage : ContentPage
{
    private readonly LeaderboardPageViewModel _leaderboardPageViewModel;
    private readonly Group _group;

    public LeaderboardPage(Group group)
    {
        InitializeComponent();

        _group = group;

        BindingContext = _leaderboardPageViewModel = new LeaderboardPageViewModel(group);
    }
}