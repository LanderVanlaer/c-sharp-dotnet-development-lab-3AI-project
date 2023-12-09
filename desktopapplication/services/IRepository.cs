using System.ComponentModel;
using desktopapplication.Models;

namespace desktopapplication.services;

public interface IRepository : INotifyPropertyChanged
{
    bool IsAuthenticated { get; }

    Task Login(string username, string password);
    Task<User> Register(string username, string password);

    ICollection<Group>? Groups();
    Task<ICollection<Group>> FetchGroups();

    ICollection<Payment>? Payments();
    Task<ICollection<Payment>> FetchPayments(Guid groupId);
    Task<Payment> GetPayment(Guid groupId, Guid paymentId);
}