using c_sharp_dotnet_development_lab_3AI_project.database.entities.group;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.user_group.dto;

public class UserGroupReadDto
{
    public Guid UserId { get; init; }
    public User User { get; set; } = null!;

    public Guid GroupId { get; init; }
    public Group Group { get; set; } = null!;
}