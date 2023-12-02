using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.payment_record;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user_group;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.user;

public class User
{
    [Key] public Guid Id { get; init; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; init; }

    [MaxLength] public string PasswordHash { get; set; }

    [MinLength(3)] [MaxLength(32)] public string Username { get; set; }

    public ICollection<UserGroup> UserGroups { get; set; }
    public ICollection<PaymentRecord> PaymentRecords { get; set; }
}