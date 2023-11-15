using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities;

public class User
{
    [Key] public Guid Id { get; init; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; init; }

    public string PasswordHash { get; set; }
    public string Username { get; set; }

    public ICollection<UserGroup> UserGroups { get; set; }
}