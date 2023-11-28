using System.ComponentModel.DataAnnotations;
using c_sharp_dotnet_development_lab_3AI_project.validators;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.payment_record.dto;

public class PaymentRecordWriteDto
{
    [Required] public decimal Amount { get; set; }
    [Required] [ValidGuid] public Guid UserId { get; init; }
}