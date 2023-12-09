namespace desktopapplication.Models;

public class PaymentRecord
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public decimal Amount { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid PaymentId { get; set; }
    public Payment? Payment { get; set; }
}