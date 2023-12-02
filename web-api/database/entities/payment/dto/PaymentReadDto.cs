using c_sharp_dotnet_development_lab_3AI_project.database.entities.group;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.payment_record;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.payment.dto;

public class PaymentReadDto
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; init; }

    public PaymentType type { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }

    public Guid GroupId { get; init; }
    public Group Group { get; set; } = null!;

    public ICollection<PaymentRecord> PaymentRecords { get; set; }
}