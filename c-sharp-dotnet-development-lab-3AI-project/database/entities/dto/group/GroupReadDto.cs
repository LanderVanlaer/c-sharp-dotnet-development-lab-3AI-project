namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.dto.group;

public class GroupReadDto
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; init; }

    public string Name { get; set; }

    public ICollection<UserGroup> UserGroups { get; set; }
    public ICollection<Payment> Payments { get; set; }
}