using c_sharp_dotnet_development_lab_3AI_project.database.entities;

namespace c_sharp_dotnet_development_lab_3AI_project.Services;

public interface IRepository
{
    IEnumerable<User> GetAllUsers();
    void AddUser(User u);

    Task<int> SaveChanges();
    User? GetUser(Guid id);
}