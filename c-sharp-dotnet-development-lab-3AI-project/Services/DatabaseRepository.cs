using c_sharp_dotnet_development_lab_3AI_project.database;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.group;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user_group;
using Microsoft.EntityFrameworkCore;

namespace c_sharp_dotnet_development_lab_3AI_project.Services;

public class DatabaseRepository : IRepository
{
    private readonly DatabaseContext _context;

    public DatabaseRepository(DatabaseContext context) => _context = context;

    public async Task<int> SaveChanges() => await _context.SaveChangesAsync();


    // ==================== USERS ====================
    public IEnumerable<User> GetAllUsers() => _context.Users;
    public User? GetUser(Guid id) => _context.Users.Find(id);
    public User? GetUserByUserName(string username) => _context.Users.FirstOrDefault(u => u.Username == username);

    public void AddUser(User u) => _context.Users.Add(u);


    // ==================== GROUPS ====================
    public IEnumerable<Group> GetAllGroupsOfUser(Guid userId) =>
        _context.Groups.Where(group => group.UserGroups.Any(ug => ug.UserId == userId));

    public Group? GetGroup(Guid id) => _context.Groups.Find(id);

    public Group? GetGroupWithUsersGroups(Guid id) =>
        _context.Groups.Include(group => group.UserGroups).FirstOrDefault(g => g.Id == id);

    public void AddGroup(Group g) => _context.Groups.Add(g);

    // ================= USER GROUPS =================
    public void AddUserGroup(UserGroup userGroup) => _context.UserGroups.Add(userGroup);
}