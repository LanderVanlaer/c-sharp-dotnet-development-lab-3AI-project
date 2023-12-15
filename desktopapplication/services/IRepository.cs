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

    Task Login(string username, string password);
    Task<User> Register(string username, string password);

    Task<ICollection<Group>> FetchGroups();
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
}

public record PaymentEntry(Guid UserId, decimal Amount);