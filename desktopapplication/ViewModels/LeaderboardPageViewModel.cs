using System.ComponentModel;
using System.Runtime.CompilerServices;
using desktopapplication.Models;

namespace desktopapplication.ViewModels;

public class LeaderboardPageViewModel : BaseViewModel
{
    private double _listViewWidth;
    private ICollection<WhoHasToPayWho> _whoHasToPayWho = new List<WhoHasToPayWho>();

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
                    CalculateWhoHasToPayWho();
                    break;
            }
        };

        LoadLeaderboardCommand = new Command(LoadLeaderboard);
        LoadLeaderboard();
    }

    public ICollection<WhoHasToPayWho> WhoHasToPayWho
    {
        get => _whoHasToPayWho;
        set => SetField(ref _whoHasToPayWho, value);
    }

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

    public Command LoadLeaderboardCommand { get; }

    public IEnumerable<LeaderboardItemWrapper> Leaderboard =>
        Repository.Leaderboard?
            .Select(item => new LeaderboardItemWrapper(item, this))
            .OrderBy(wrapper => Math.Abs(wrapper.LeaderboardItem.Amount)) ??
        Enumerable.Empty<LeaderboardItemWrapper>();

    public decimal MaxAmount { get; private set; }

    private void CalculateWhoHasToPayWho()
    {
        if (Repository.Leaderboard is null)
        {
            WhoHasToPayWho = [];
            return;
        }

        Queue<InternalLeaderboardItemStruct> hasToGetPayedList =
            new(Repository.Leaderboard
                .Where(leaderboardItem => leaderboardItem.Amount > 0)
                .OrderByDescending(item => item.Amount)
                .ThenBy(item => item.UserId)
                .Select(item => new InternalLeaderboardItemStruct(item))
            );
        Queue<InternalLeaderboardItemStruct> hasToPayList =
            new(Repository.Leaderboard
                .Where(leaderboardItem => leaderboardItem.Amount < 0)
                .OrderBy(item => item.Amount)
                .ThenBy(item => item.UserId)
                .Select(item => new InternalLeaderboardItemStruct(item))
            );

        List<WhoHasToPayWho> whoHasToPayWho = [];

        InternalLeaderboardItemStruct? needsMoney = hasToGetPayedList.Dequeue();
        InternalLeaderboardItemStruct? givesMoney = hasToPayList.Dequeue();

        while (needsMoney is not null && givesMoney is not null)
        {
            decimal amount = Math.Min(needsMoney.Amount, Math.Abs(givesMoney.Amount));

            whoHasToPayWho.Add(new WhoHasToPayWho
            {
                WhoUserId = givesMoney.UserId,
                ToWhomUserId = needsMoney.UserId,
                Amount = amount,
            });

            if (needsMoney.Amount == amount)
                needsMoney = hasToGetPayedList.Count > 0 ? hasToGetPayedList.Dequeue() : null;
            else
                needsMoney.Amount -= amount;

            if (givesMoney.Amount == -amount)
                givesMoney = hasToPayList.Count > 0 ? hasToPayList.Dequeue() : null;
            else
                givesMoney.Amount += amount;
        }

        WhoHasToPayWho = whoHasToPayWho;
    }

    private void LoadLeaderboard() => Task.Run(async () =>
    {
        await LoadOnTask(Repository.FetchLeaderboard(Group.Id));
        OnPropertyChanged(nameof(Leaderboard));
    });
}

public record LeaderboardItemWrapper : INotifyPropertyChanged
{
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

    public LeaderboardItem LeaderboardItem { get; }
    private LeaderboardPageViewModel LeaderboardPageViewModel { get; }

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
}

public record WhoHasToPayWho
{
    public Guid WhoUserId { get; init; }
    public Guid ToWhomUserId { get; init; }
    public decimal Amount { get; init; }
}

internal record InternalLeaderboardItemStruct
{
    public InternalLeaderboardItemStruct(LeaderboardItem item)
    {
        UserId = item.UserId;
        Amount = item.Amount;
    }

    public Guid UserId { get; init; }
    public decimal Amount { get; set; }
}