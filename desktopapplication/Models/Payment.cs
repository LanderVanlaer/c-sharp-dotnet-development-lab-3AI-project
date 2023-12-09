namespace desktopapplication.Models;

public class Payment
{
    public enum PaymentType
    {
        PURCHASE = 1,
        REPAYMENT = 2,
    }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public PaymentType Type { get; set; }
    public Guid GroupId { get; set; }
}