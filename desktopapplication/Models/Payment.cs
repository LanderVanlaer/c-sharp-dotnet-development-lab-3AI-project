namespace desktopapplication.Models;

public class Payment
{
    public enum PaymentType
    {
        Purchase = 1,
        Repayment = 2,
    }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public PaymentType Type { get; set; }
    public Guid GroupId { get; set; }
    public Group? Group { get; set; }

    public ICollection<PaymentRecord>? PaymentRecords { get; set; }
}