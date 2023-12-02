using System.ComponentModel.DataAnnotations;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.group.dto;

public class GroupWriteDto
{
    [Required]
    [StringLength(32,
        ErrorMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.",
        MinimumLength = 3)]
    public string Name { get; set; }
}