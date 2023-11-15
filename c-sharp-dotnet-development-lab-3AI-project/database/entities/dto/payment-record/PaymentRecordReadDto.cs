﻿namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.dto.payment_record;

public class PaymentRecordReadDto
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; init; }

    public Guid UserId { get; init; }
    public User User { get; set; } = null!;

    public Guid PaymentId { get; init; }
    public Payment Payment { get; set; } = null!;
}