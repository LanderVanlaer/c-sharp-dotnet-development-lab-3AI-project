using System.ComponentModel.DataAnnotations;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.user.dto;

public class UserWriteDto
{
    [Required]
    [StringLength(18,
        ErrorMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.",
        MinimumLength = 6)]
    [RegularExpression("^[a-zA-Z0-9]+$",
        ErrorMessage = "The field {0} must be a string with only letters and numbers.")]
    public string Username { get; set; }

    [Required]
    [StringLength(32,
        ErrorMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.",
        MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
        ErrorMessage =
            "The field {0} must have at least one uppercase letter, one lowercase letter, one number and one special character.")]
    public string Password { get; set; }
}