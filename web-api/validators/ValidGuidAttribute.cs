using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace c_sharp_dotnet_development_lab_3AI_project.validators;

/// <summary>
///     https://stackoverflow.com/a/7187711/13165967
/// </summary>
public class ValidGuidAttribute : ValidationAttribute
{
    private const string DefaultErrorMessage = "'{0}' does not contain a valid guid";

    public ValidGuidAttribute() : base(DefaultErrorMessage)
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string? input = Convert.ToString(value, CultureInfo.CurrentCulture);

        // let the Required attribute take care of this validation
        if (string.IsNullOrWhiteSpace(input))
            return null;

        if (Guid.Empty.Equals(value))
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

        return Guid.TryParse(input, out _)
            ? null
            : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    }
}