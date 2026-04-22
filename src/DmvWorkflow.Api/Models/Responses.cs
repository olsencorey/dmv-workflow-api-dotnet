namespace DmvWorkflow.Api.Models;

public record StartSessionResponse(Guid SessionId, string Status);
public record LookupVehicleResponse(Guid SessionId, string Status, VehicleRecord Vehicle, OwnerRecord Owner);
public record QuoteResponse(Guid SessionId, string Status, RenewalQuote Quote);
public record PaymentResponse(Guid SessionId, string Status, PaymentTransaction Payment);
public record FinalizeResponse(Guid SessionId, string Status, RenewalReceipt Receipt);
