namespace desktopapplication.Models;

public class Group
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }

    public string Name { get; set; }
}