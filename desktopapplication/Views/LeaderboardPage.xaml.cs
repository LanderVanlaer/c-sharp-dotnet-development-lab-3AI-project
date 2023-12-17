using desktopapplication.Models;
using desktopapplication.ViewModels;

namespace desktopapplication.Views;

public partial class LeaderboardPage : ContentPage
{
    private readonly Group _group;
    private readonly LeaderboardPageViewModel _leaderboardPageViewModel;

    public LeaderboardPage(Group group)
    {
        InitializeComponent();

        _group = group;

        BindingContext = _leaderboardPageViewModel = new LeaderboardPageViewModel(group);
    }
}