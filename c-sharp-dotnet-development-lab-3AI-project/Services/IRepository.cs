using c_sharp_dotnet_development_lab_3AI_project.database.entities.group;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.leaderboard.dto;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.payment;
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

    IEnumerable<User> GetUsersByGroupId(Guid groupId);

    void AddUser(User u);

    // ==================== GROUPS ====================
    IEnumerable<Group> GetAllGroupsOfUser(Guid userId);
    Group? GetGroup(Guid id);
    Group? GetGroupWithUsersGroups(Guid id);
    bool UserHasAccessToGroup(Guid userId, Guid groupId);

    void AddGroup(Group g);

    // ================= USER GROUPS =================
    void AddUserGroup(UserGroup userGroup);

    // =================== PAYMENTS ===================
    Payment? GetPayment(Guid paymentId);
    Payment? GetPaymentWithPaymentRecords(Guid paymentId);
    void AddPayment(Payment payment);
    void UpdatePayment(Payment payment);
    IEnumerable<Payment> GetPaymentsOfGroup(Guid groupId);

    // =============== PAYMENT RECORDS ===============
    int DeletePaymentRecordsOfPayment(Guid paymentId);

    // ================= LEADERBOARD =================
    IEnumerable<LeaderboardItemReadDto> GetLeaderboardOfGroup(Guid groupId);
}