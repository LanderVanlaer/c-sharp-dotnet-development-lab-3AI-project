using Microsoft.EntityFrameworkCore;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities;

[PrimaryKey(nameof(UserId), nameof(GroupId))]
public class UserGroup
{
    public Guid UserId { get; init; }
    public User User { get; set; } = null!;

    public Guid GroupId { get; init; }
    public Group Group { get; set; } = null!;
}