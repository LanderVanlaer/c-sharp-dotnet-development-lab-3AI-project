﻿using c_sharp_dotnet_development_lab_3AI_project.database;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.group;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.leaderboard.dto;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.payment;
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

    public IEnumerable<User> GetUsersByGroupId(Guid groupId) =>
        _context.Users.Where(u => u.UserGroups.Any(ug => ug.GroupId == groupId));

    public void AddUser(User u) => _context.Users.Add(u);


    // ==================== GROUPS ====================
    public IEnumerable<Group> GetAllGroupsOfUser(Guid userId) =>
        _context.Groups.Where(group => group.UserGroups.Any(ug => ug.UserId == userId));

    public Group? GetGroup(Guid id) => _context.Groups.Find(id);

    public Group? GetGroupWithUsersGroups(Guid id) =>
        _context.Groups.Include(group => group.UserGroups).FirstOrDefault(g => g.Id == id);

    public bool UserHasAccessToGroup(Guid userId, Guid groupId) =>
        _context.UserGroups.Any(ug => ug.UserId == userId && ug.GroupId == groupId);

    public void AddGroup(Group g) => _context.Groups.Add(g);

    // ================= USER GROUPS =================
    public void AddUserGroup(UserGroup userGroup) => _context.UserGroups.Add(userGroup);

    // =================== PAYMENTS ===================
    public Payment? GetPayment(Guid paymentId) => _context.Payments.Find(paymentId);

    public Payment? GetPaymentWithPaymentRecords(Guid paymentId) => _context.Payments
        .Include(payment => payment.PaymentRecords)
        .FirstOrDefault(payment => payment.Id == paymentId);

    public void AddPayment(Payment payment) => _context.Payments.Add(payment);

    public void UpdatePayment(Payment payment) => _context.Payments.Update(payment);

    public IEnumerable<Payment> GetPaymentsOfGroup(Guid groupId) =>
        _context.Payments.Where(payment => payment.GroupId == groupId);


    // =============== PAYMENT RECORDS ===============
    public int DeletePaymentRecordsOfPayment(Guid paymentId) =>
        _context.PaymentRecords.Where(pr => pr.PaymentId == paymentId).ExecuteDelete();


    // ================= LEADERBOARD =================
    public IEnumerable<LeaderboardItemReadDto> GetLeaderboardOfGroup(Guid groupId)
    {
        /*
         * SELECT U.Id, SUM(PR.Amount) AS TotalPaid
         * FROM Users U
         *          INNER JOIN app.UserGroups UG on U.Id = UG.UserId
         *          LEFT JOIN app.PaymentRecords PR on U.Id = PR.UserId
         *          LEFT JOIN app.Payments P on PR.PaymentId = P.Id
         * WHERE UG.GroupId = 'groupId'
         *   AND (P.GroupId = 'groupId' OR P.GroupId IS NULL)
         * GROUP BY U.Id
         */
        return _context.Users
            .Include(user => user.UserGroups)
            .Include(user => user.PaymentRecords)
            .ThenInclude(paymentRecord => paymentRecord.Payment)
            .Where(user => user.UserGroups.Any(userGroup => userGroup.GroupId == groupId))
            .Select(user => new LeaderboardItemReadDto
            {
                UserId = user.Id,
                Amount = user.PaymentRecords
                    .Where(paymentRecord => paymentRecord.Payment.GroupId == groupId)
                    .Sum(paymentRecord => paymentRecord.Amount),
            }).ToArray();
    }
}