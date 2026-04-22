namespace DmvWorkflow.Api.Models;

public class RenewalQuote
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SessionId { get; set; }
    public int Months { get; set; }
    public decimal RegistrationFee { get; set; }
    public decimal CountyFee { get; set; }
    public decimal ProcessingFee { get; set; }
    public decimal Total => RegistrationFee + CountyFee + ProcessingFee;
    public DeliveryMethodType DeliveryMethod { get; set; }
    public DateTimeOffset ExpiresAtUtc { get; set; } = DateTimeOffset.UtcNow.AddMinutes(15);
}
