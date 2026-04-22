using DmvWorkflow.Api.Data;
using DmvWorkflow.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DmvWorkflow.Api.Repositories;

public class EfDmvRepository : IDmvRepository
{
    private readonly DmvWorkflowDbContext _db;

    public EfDmvRepository(DmvWorkflowDbContext db)
    {
        _db = db;
    }

    public RenewalSession AddSession(RenewalSession session)
    {
        _db.RenewalSessions.Add(session);
        _db.SaveChanges();
        return session;
    }

    public RenewalSession? GetSession(Guid sessionId) =>
        _db.RenewalSessions.FirstOrDefault(x => x.Id == sessionId);

    public void UpdateSession(RenewalSession session)
    {
        _db.RenewalSessions.Update(session);
        _db.SaveChanges();
    }

    public VehicleRecord? FindVehicle(string? noticeNumber, string? plateNumber, string? vinLast6) =>
        _db.Vehicles.FirstOrDefault(v =>
            (!string.IsNullOrWhiteSpace(noticeNumber) && v.NoticeNumber == noticeNumber) ||
            (!string.IsNullOrWhiteSpace(plateNumber) && !string.IsNullOrWhiteSpace(vinLast6) &&
             v.PlateNumber == plateNumber && v.VinLast6 == vinLast6));

    public VehicleRecord? GetVehicle(Guid vehicleId) =>
        _db.Vehicles.FirstOrDefault(x => x.Id == vehicleId);

    public OwnerRecord? GetOwner(Guid ownerId) =>
        _db.Owners.FirstOrDefault(x => x.Id == ownerId);

    public RenewalQuote AddQuote(RenewalQuote quote)
    {
        _db.RenewalQuotes.Add(quote);
        _db.SaveChanges();
        return quote;
    }

    public RenewalQuote? GetQuote(Guid quoteId) =>
        _db.RenewalQuotes.FirstOrDefault(x => x.Id == quoteId);

    public PaymentTransaction AddPayment(PaymentTransaction payment)
    {
        _db.PaymentTransactions.Add(payment);
        _db.SaveChanges();
        return payment;
    }

    public PaymentTransaction? GetPayment(Guid paymentId) =>
        _db.PaymentTransactions.FirstOrDefault(x => x.Id == paymentId);

    public RenewalReceipt AddReceipt(RenewalReceipt receipt)
    {
        _db.RenewalReceipts.Add(receipt);
        _db.SaveChanges();
        return receipt;
    }

    public RenewalReceipt? GetReceipt(string receiptNumber) =>
        _db.RenewalReceipts.FirstOrDefault(x => x.ReceiptNumber == receiptNumber);
}
