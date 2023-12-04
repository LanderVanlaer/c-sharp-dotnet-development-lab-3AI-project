using desktopapplication.Models;

namespace desktopapplication.services;

public interface IRepository
{
    ICollection<Group>? Groups();
    Task<ICollection<Group>> FetchGroups();
}