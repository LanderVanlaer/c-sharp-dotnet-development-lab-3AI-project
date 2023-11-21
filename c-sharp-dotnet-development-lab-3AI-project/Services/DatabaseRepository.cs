using c_sharp_dotnet_development_lab_3AI_project.database;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user;

namespace c_sharp_dotnet_development_lab_3AI_project.Services;

public class DatabaseRepository : IRepository
{
    private readonly DatabaseContext _context;

    public DatabaseRepository(DatabaseContext context) => _context = context;

    public IEnumerable<User> GetAllUsers() => _context.Users;

    public void AddUser(User u) => _context.Users.Add(u);

    public async Task<int> SaveChanges() => await _context.SaveChangesAsync();

    public User? GetUser(Guid id) => _context.Users.Find(id);
}