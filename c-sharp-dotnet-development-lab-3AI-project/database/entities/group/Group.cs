using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.payment;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user_group;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.group;

public class Group
{
    [Key] public Guid Id { get; init; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; init; }

    [MaxLength(32)] public string Name { get; set; }

    public ICollection<UserGroup> UserGroups { get; set; }
    public ICollection<Payment> Payments { get; set; }

    public bool IncludesUser(Guid userId) => UserGroups.Any(userGroup => userGroup.UserId == userId);
}