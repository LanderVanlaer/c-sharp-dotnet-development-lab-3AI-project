using c_sharp_dotnet_development_lab_3AI_project.database.entities.group;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user_group;

namespace c_sharp_dotnet_development_lab_3AI_project.Services;

public interface IRepository
{
    Task<int> SaveChanges();

    // ==================== USERS ====================
    IEnumerable<User> GetAllUsers();
    User? GetUser(Guid id);
    User? GetUserByUserName(string username);

    void AddUser(User u);

    // ==================== GROUPS ====================
    IEnumerable<Group> GetAllGroupsOfUser(Guid userId);
    Group? GetGroup(Guid id);
    Group? GetGroupWithUsersGroups(Guid id);

    void AddGroup(Group g);

    // ================= USER GROUPS =================
    void AddUserGroup(UserGroup userGroup);
}