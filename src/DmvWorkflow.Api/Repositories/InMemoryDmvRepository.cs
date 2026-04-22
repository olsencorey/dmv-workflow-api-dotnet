using DmvWorkflow.Api.Models;

namespace DmvWorkflow.Api.Repositories;

public class InMemoryDmvRepository : IDmvRepository
{
    private readonly List<OwnerRecord> _owners = new();
    private readonly List<VehicleRecord> _vehicles = new();
    private readonly List<RenewalSession> _sessions = new();
    private readonly List<RenewalQuote> _quotes = new();
    private readonly List<PaymentTransaction> _payments = new();
    private readonly List<RenewalReceipt> _receipts = new();

    public InMemoryDmvRepository()
    {
        var owner = new OwnerRecord
        {
            FullName = "Jordan Ramirez",
            AddressLine1 = "1450 Desert Willow Ave",
            City = "Phoenix",
            State = "AZ",
            PostalCode = "85004"
        };
        _owners.Add(owner);

        _vehicles.Add(new VehicleRecord
        {
            OwnerId = owner.Id,
            PlateNumber = "AZX4219",
            VinLast6 = "941203",
            NoticeNumber = "RN-2026-0001",
            Make = "Toyota",
            Model = "Camry",
            Year = 2021,
            RegistrationExpiresOn = new DateOnly(2026, 5, 31),
            EmissionsHold = false,
            InsuranceVerified = true
        });
    }

    public RenewalSession AddSession(RenewalSession session) { _sessions.Add(session); return session; }
    public RenewalSession? GetSession(Guid sessionId) => _sessions.FirstOrDefault(x => x.Id == sessionId);
    public void UpdateSession(RenewalSession session) { }
    public VehicleRecord? FindVehicle(string? noticeNumber, string? plateNumber, string? vinLast6) =>
        _vehicles.FirstOrDefault(v =>
            (!string.IsNullOrWhiteSpace(noticeNumber) && v.NoticeNumber.Equals(noticeNumber, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrWhiteSpace(plateNumber) && !string.IsNullOrWhiteSpace(vinLast6) &&
             v.PlateNumber.Equals(plateNumber, StringComparison.OrdinalIgnoreCase) &&
             v.VinLast6.Equals(vinLast6, StringComparison.OrdinalIgnoreCase)));
    public VehicleRecord? GetVehicle(Guid vehicleId) => _vehicles.FirstOrDefault(x => x.Id == vehicleId);
    public OwnerRecord? GetOwner(Guid ownerId) => _owners.FirstOrDefault(x => x.Id == ownerId);
    public RenewalQuote AddQuote(RenewalQuote quote) { _quotes.Add(quote); return quote; }
    public RenewalQuote? GetQuote(Guid quoteId) => _quotes.FirstOrDefault(x => x.Id == quoteId);
    public PaymentTransaction AddPayment(PaymentTransaction payment) { _payments.Add(payment); return payment; }
    public PaymentTransaction? GetPayment(Guid paymentId) => _payments.FirstOrDefault(x => x.Id == paymentId);
    public RenewalReceipt AddReceipt(RenewalReceipt receipt) { _receipts.Add(receipt); return receipt; }
    public RenewalReceipt? GetReceipt(string receiptNumber) => _receipts.FirstOrDefault(x => x.ReceiptNumber.Equals(receiptNumber, StringComparison.OrdinalIgnoreCase));
}
