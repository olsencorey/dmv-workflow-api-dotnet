namespace DmvWorkflow.Api.Models;

public class RenewalReceipt
{
    public string ReceiptNumber { get; set; } = string.Empty;
    public Guid SessionId { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public DateOnly NewExpirationDate { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTimeOffset IssuedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DeliveryMethodType DeliveryMethod { get; set; }
}
