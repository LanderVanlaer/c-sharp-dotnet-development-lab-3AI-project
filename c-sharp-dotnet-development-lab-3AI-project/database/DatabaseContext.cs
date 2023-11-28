using c_sharp_dotnet_development_lab_3AI_project.database.entities.group;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.payment;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.payment_record;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user_group;
using Microsoft.EntityFrameworkCore;

namespace c_sharp_dotnet_development_lab_3AI_project.database;

/// <summary>
///     https://medium.com/@darshana-edirisinghe/entity-framework-performance-improvement-section-1-different-loading-mechanisms-in-entity-3e3ce2affee6
/// </summary>
public sealed class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) =>
        ChangeTracker.LazyLoadingEnabled = false;

    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<PaymentRecord> PaymentRecords { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserGroup> UserGroups { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(user => user.Username)
            .IsUnique();
    }
}