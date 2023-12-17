using System.ComponentModel;
using desktopapplication.Models;

namespace desktopapplication.services;

public interface IRepository : INotifyPropertyChanged
{
    bool IsAuthenticated { get; }


    ICollection<Group>? Groups { get; }
    User? User { get; }
    ICollection<User>? Users { get; }
    ICollection<Payment>? Payments { get; }

    ICollection<LeaderboardItem>? Leaderboard { get; }

    Task Login(string username, string password);
    Task<User> Register(string username, string password);
    void Logout();

    Task<ICollection<Group>> FetchGroups();
    Task<Group> CreateGroup(string name);
    Task AddUserToGroup(Guid groupId, string username);

    Task<User> FetchUser();
    Task<ICollection<User>> FetchUsers(Guid groupId);
    Task<User> UpdateUser(string? username, string? password);

    Task<ICollection<Payment>> FetchPayments(Guid groupId);
    Task<Payment> GetPayment(Guid groupId, Guid paymentId);

    Task<Payment> AddPayment(Guid groupId,
        Payment.PaymentType type,
        string name,
        string description,
        IEnumerable<PaymentEntry> paymentEntries);

    Task<ICollection<LeaderboardItem>> FetchLeaderboard(Guid groupId);
}

public record PaymentEntry(Guid UserId, decimal Amount);