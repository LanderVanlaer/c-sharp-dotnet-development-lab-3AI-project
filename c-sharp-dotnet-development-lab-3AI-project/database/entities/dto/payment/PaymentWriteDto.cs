namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.dto.payment;

public class PaymentWriteDto
{
    public PaymentType type { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }

    public Guid GroupId { get; init; }
}