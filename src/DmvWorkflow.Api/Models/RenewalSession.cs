namespace DmvWorkflow.Api.Models;

public class RenewalSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public ChannelType Channel { get; set; }
    public string? KioskId { get; set; }
    public SessionStatus Status { get; set; } = SessionStatus.Started;
    public Guid? VehicleId { get; set; }
    public Guid? QuoteId { get; set; }
    public Guid? PaymentId { get; set; }
    public string? ReceiptNumber { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public List<AuditEvent> AuditTrail { get; set; } = new();
}
