using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities;

public class PaymentRecord
{
    [Key] public Guid Id { get; init; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; init; }

    public Guid UserId { get; init; }
    public User User { get; set; } = null!;

    public Guid PaymentId { get; init; }
    public Payment Payment { get; set; } = null!;
}