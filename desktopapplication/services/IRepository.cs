using System.ComponentModel;
using desktopapplication.Models;

namespace desktopapplication.services;

public interface IRepository : INotifyPropertyChanged
{
    bool IsAuthenticated { get; }

    Task Login(string username, string password);
    Task<User> Register(string username, string password);


    ICollection<Group>? Groups { get; }
    Task<ICollection<Group>> FetchGroups();


    User? User { get; }
    Task<User> FetchUser();

    ICollection<User>? Users { get; }
    Task<ICollection<User>> FetchUsers(Guid groupId);
    Task<User> UpdateUser(string? username, string? password);


    ICollection<Payment>? Payments { get; }
    Task<ICollection<Payment>> FetchPayments(Guid groupId);
    Task<Payment> GetPayment(Guid groupId, Guid paymentId);
}