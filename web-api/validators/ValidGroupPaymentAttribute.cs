using System.ComponentModel.DataAnnotations;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.payment;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.payment_record;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.payment.dto;

namespace c_sharp_dotnet_development_lab_3AI_project.validators;

public class ValidGroupPaymentAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not PaymentWriteDto dto)
            return null;

        if (dto.PaymentRecords.Length == 0)
            return new ValidationResult(nameof(Payment.PaymentRecords) + " cannot be empty");

        if (dto.PaymentRecords.Length != dto.PaymentRecords.Select(record => record.UserId).Distinct().Count())
            return new ValidationResult(nameof(Payment.PaymentRecords) +
                                        " cannot contain duplicate " +
                                        nameof(PaymentRecord.UserId));

        if (dto.PaymentRecords.Select(paymentRecordWriteDto => paymentRecordWriteDto.Amount).Sum() > 0.01m)
            return new ValidationResult(nameof(Payment.PaymentRecords) + " amounts must sum to 0");

        if (dto.PaymentRecords.Select(paymentRecordWriteDto => paymentRecordWriteDto.Amount == 0).Any())
            return new ValidationResult(nameof(Payment.PaymentRecords) + " cannot contain users with an amount of 0");

        return ValidationResult.Success;
    }
}