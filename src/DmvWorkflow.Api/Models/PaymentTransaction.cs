namespace DmvWorkflow.Api.Models;

public class PaymentTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SessionId { get; set; }
    public PaymentMethodType PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public string? Last4 { get; set; }
    public string AuthorizationCode { get; set; } = string.Empty;
    public DateTimeOffset AuthorizedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
