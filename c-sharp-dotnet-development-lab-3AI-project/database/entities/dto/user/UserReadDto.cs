namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.dto.user;

public class UserReadDto
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; init; }

    public string Username { get; set; }

    public ICollection<UserGroup> UserGroups { get; set; }
}