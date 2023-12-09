using System.ComponentModel;
using desktopapplication.Models;

namespace desktopapplication.services;

public interface IRepository : INotifyPropertyChanged
{
    bool IsAuthenticated { get; }
    ICollection<Group>? Groups();
    Task<ICollection<Group>> FetchGroups();
    Task Login(string username, string password);
    Task<User> Register(string username, string password);
}