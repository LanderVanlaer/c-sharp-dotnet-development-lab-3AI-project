using System.ComponentModel;
using System.Runtime.CompilerServices;
using desktopapplication.Models;

namespace desktopapplication.ViewModels;

public class LeaderboardPageViewModel : BaseViewModel
{
    private double _listViewWidth;
    private Group Group { get; }

    public double ListViewWidth
    {
        get => _listViewWidth;
        set
        {
            if (value.Equals(_listViewWidth)) return;

            SetField(ref _listViewWidth, value);
            OnPropertyChanged(nameof(LeaderboardItemWrapper.PixelWidth));
        }
    }

    public LeaderboardPageViewModel(Group group)
    {
        Group = group;

        if (Repository.Users is null) Repository.FetchUsers(group.Id);

        Repository.PropertyChanged += (_, args) =>
        {
            switch (args.PropertyName)
            {
                case nameof(Repository.Leaderboard):
                    OnPropertyChanged(nameof(Leaderboard));
                    MaxAmount = Math.Abs(Leaderboard.Last().LeaderboardItem.Amount);
                    break;
            }
        };

        LoadLeaderboardCommand = new Command(LoadLeaderboard);
        LoadLeaderboard();
    }

    public Command LoadLeaderboardCommand { get; }

    public IEnumerable<LeaderboardItemWrapper> Leaderboard =>
        Repository.Leaderboard?
            .Select(item => new LeaderboardItemWrapper(item, this))
            .OrderBy(wrapper => Math.Abs(wrapper.LeaderboardItem.Amount)) ??
        Enumerable.Empty<LeaderboardItemWrapper>();

    private void LoadLeaderboard() => Task.Run(async () =>
    {
        await LoadOnTask(Repository.FetchLeaderboard(Group.Id));
        OnPropertyChanged(nameof(Leaderboard));
    });

    public decimal MaxAmount { get; private set; }
}

public record LeaderboardItemWrapper : INotifyPropertyChanged
{
    public LeaderboardItem LeaderboardItem { get; }
    private LeaderboardPageViewModel LeaderboardPageViewModel { get; }

    public LeaderboardItemWrapper(LeaderboardItem LeaderboardItem, LeaderboardPageViewModel LeaderboardPageViewModel)
    {
        this.LeaderboardItem = LeaderboardItem;
        this.LeaderboardPageViewModel = LeaderboardPageViewModel;

        LeaderboardPageViewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(LeaderboardPageViewModel.ListViewWidth))
                OnPropertyChanged(nameof(PixelWidth));
        };
    }

    public int PixelWidth
    {
        get
        {
            if (Application.Current != null)
                return (int)(LeaderboardPageViewModel.ListViewWidth / 2 * 0.95d /
                             (double)LeaderboardPageViewModel.MaxAmount *
                             (double)Math.Abs(LeaderboardItem.Amount));
            return 0;
        }
    }

    public bool IsPositive => LeaderboardItem.Amount >= 0;
    public bool IsNegative => LeaderboardItem.Amount < 0;
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
};