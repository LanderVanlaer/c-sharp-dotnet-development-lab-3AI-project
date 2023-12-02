using System.ComponentModel.DataAnnotations;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.payment_record.dto;
using c_sharp_dotnet_development_lab_3AI_project.validators;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.payment.dto;

[ValidGroupPayment]
public class PaymentWriteDto
{
    [Required]
    [EnumDataType(typeof(PaymentType))]
    public PaymentType Type { get; init; }

    [Required]
    [StringLength(64,
        ErrorMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.",
        MinimumLength = 3)]
    public string Name { get; set; }

    [Required] public string Description { get; set; }

    [Required] public PaymentRecordWriteDto[] PaymentRecords { get; set; } = null!;
}