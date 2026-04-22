using DmvWorkflow.Api.Models;

namespace DmvWorkflow.Api.Repositories;

public interface IDmvRepository
{
    RenewalSession AddSession(RenewalSession session);
    RenewalSession? GetSession(Guid sessionId);
    void UpdateSession(RenewalSession session);
    VehicleRecord? FindVehicle(string? noticeNumber, string? plateNumber, string? vinLast6);
    VehicleRecord? GetVehicle(Guid vehicleId);
    OwnerRecord? GetOwner(Guid ownerId);
    RenewalQuote AddQuote(RenewalQuote quote);
    RenewalQuote? GetQuote(Guid quoteId);
    PaymentTransaction AddPayment(PaymentTransaction payment);
    PaymentTransaction? GetPayment(Guid paymentId);
    RenewalReceipt AddReceipt(RenewalReceipt receipt);
    RenewalReceipt? GetReceipt(string receiptNumber);
}
