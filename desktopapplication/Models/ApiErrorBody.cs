namespace desktopapplication.Models;

// ReSharper disable once UnassignedField.Global
public record ApiErrorBody
{
    public required ICollection<string>? Errors;
    public required int Status;
    public required string Title;
}