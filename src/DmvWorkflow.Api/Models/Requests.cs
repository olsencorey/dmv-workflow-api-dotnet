namespace DmvWorkflow.Api.Models;

public record StartSessionRequest(string Channel, string? KioskId);
public record LookupVehicleRequest(string? NoticeNumber, string? PlateNumber, string? VinLast6);
public record CreateQuoteRequest(int Months, string DeliveryMethod);
public record SubmitPaymentRequest(string PaymentMethod, decimal Amount, string? Last4);
