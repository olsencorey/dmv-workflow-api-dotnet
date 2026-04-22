using DmvWorkflow.Api.Models;
using DmvWorkflow.Api.Repositories;

namespace DmvWorkflow.Api.Services;

public class RenewalWorkflowService : IRenewalWorkflowService
{
    private readonly IDmvRepository _repository;

    public RenewalWorkflowService(IDmvRepository repository)
    {
        _repository = repository;
    }

    public StartSessionResponse StartSession(StartSessionRequest request)
    {
        var channel = Enum.Parse<ChannelType>(request.Channel, true);
        var session = new RenewalSession
        {
            Channel = channel,
            KioskId = request.KioskId
        };

        session.AuditTrail.Add(new AuditEvent
        {
            EventType = "SessionStarted",
            Detail = $"Channel={channel}; KioskId={request.KioskId}"
        });

        _repository.AddSession(session);
        return new StartSessionResponse(session.Id, session.Status.ToString());
    }

    public LookupVehicleResponse LookupVehicle(Guid sessionId, LookupVehicleRequest request)
    {
        var session = GetRequiredSession(sessionId);
        var vehicle = _repository.FindVehicle(request.NoticeNumber, request.PlateNumber, request.VinLast6)
            ?? throw new InvalidOperationException("Vehicle record not found.");

        if (vehicle.EmissionsHold)
            throw new InvalidOperationException("Vehicle has an emissions hold and cannot be renewed at self-service channels.");

        if (!vehicle.InsuranceVerified)
            throw new InvalidOperationException("Insurance verification is required before renewal.");

        var owner = _repository.GetOwner(vehicle.OwnerId) ?? throw new InvalidOperationException("Owner record missing.");

        session.VehicleId = vehicle.Id;
        session.Status = SessionStatus.VehicleMatched;
        session.AuditTrail.Add(new AuditEvent
        {
            EventType = "VehicleMatched",
            Detail = $"Plate={vehicle.PlateNumber}; Notice={vehicle.NoticeNumber}"
        });
        _repository.UpdateSession(session);

        return new LookupVehicleResponse(session.Id, session.Status.ToString(), vehicle, owner);
    }

    public QuoteResponse CreateQuote(Guid sessionId, CreateQuoteRequest request)
    {
        var session = GetRequiredSession(sessionId);
        if (session.VehicleId is null)
            throw new InvalidOperationException("Vehicle must be matched before quoting.");

        var deliveryMethod = Enum.Parse<DeliveryMethodType>(request.DeliveryMethod, true);
        var registrationFee = request.Months switch
        {
            12 => 92.00m,
            24 => 176.00m,
            _ => throw new InvalidOperationException("Only 12 or 24 month renewals are supported.")
        };

        var quote = new RenewalQuote
        {
            SessionId = session.Id,
            Months = request.Months,
            RegistrationFee = registrationFee,
            CountyFee = 22.50m,
            ProcessingFee = deliveryMethod == DeliveryMethodType.PrintAtKiosk ? 4.00m : 2.50m,
            DeliveryMethod = deliveryMethod
        };

        _repository.AddQuote(quote);
        session.QuoteId = quote.Id;
        session.Status = SessionStatus.Quoted;
        session.AuditTrail.Add(new AuditEvent
        {
            EventType = "QuoteCreated",
            Detail = $"Months={quote.Months}; Total={quote.Total}"
        });
        _repository.UpdateSession(session);

        return new QuoteResponse(session.Id, session.Status.ToString(), quote);
    }

    public PaymentResponse SubmitPayment(Guid sessionId, SubmitPaymentRequest request)
    {
        var session = GetRequiredSession(sessionId);
        if (session.QuoteId is null)
            throw new InvalidOperationException("Quote must be created before payment.");

        var quote = _repository.GetQuote(session.QuoteId.Value) ?? throw new InvalidOperationException("Quote not found.");
        if (request.Amount != quote.Total)
            throw new InvalidOperationException($"Payment amount must equal quote total of {quote.Total}.");

        var payment = new PaymentTransaction
        {
            SessionId = session.Id,
            PaymentMethod = Enum.Parse<PaymentMethodType>(request.PaymentMethod, true),
            Amount = request.Amount,
            Last4 = request.Last4,
            AuthorizationCode = $"AUTH-{Random.Shared.Next(100000, 999999)}"
        };

        _repository.AddPayment(payment);
        session.PaymentId = payment.Id;
        session.Status = SessionStatus.PaymentAuthorized;
        session.AuditTrail.Add(new AuditEvent
        {
            EventType = "PaymentAuthorized",
            Detail = $"Amount={payment.Amount}; Auth={payment.AuthorizationCode}"
        });
        _repository.UpdateSession(session);

        return new PaymentResponse(session.Id, session.Status.ToString(), payment);
    }

    public FinalizeResponse Finalize(Guid sessionId)
    {
        var session = GetRequiredSession(sessionId);
        if (session.VehicleId is null || session.QuoteId is null || session.PaymentId is null)
            throw new InvalidOperationException("Lookup, quote, and payment must be completed before finalization.");

        var vehicle = _repository.GetVehicle(session.VehicleId.Value) ?? throw new InvalidOperationException("Vehicle not found.");
        var quote = _repository.GetQuote(session.QuoteId.Value) ?? throw new InvalidOperationException("Quote not found.");
        var payment = _repository.GetPayment(session.PaymentId.Value) ?? throw new InvalidOperationException("Payment not found.");

        var currentBase = vehicle.RegistrationExpiresOn > DateOnly.FromDateTime(DateTime.UtcNow)
            ? vehicle.RegistrationExpiresOn
            : DateOnly.FromDateTime(DateTime.UtcNow);
        var newExpiration = currentBase.AddMonths(quote.Months);
        vehicle.RegistrationExpiresOn = newExpiration;

        var receipt = new RenewalReceipt
        {
            ReceiptNumber = $"RCPT-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}",
            SessionId = session.Id,
            PlateNumber = vehicle.PlateNumber,
            NewExpirationDate = newExpiration,
            AmountPaid = payment.Amount,
            DeliveryMethod = quote.DeliveryMethod
        };

        _repository.AddReceipt(receipt);
        session.ReceiptNumber = receipt.ReceiptNumber;
        session.Status = SessionStatus.Completed;
        session.AuditTrail.Add(new AuditEvent
        {
            EventType = "RenewalCompleted",
            Detail = $"Receipt={receipt.ReceiptNumber}; NewExpiration={receipt.NewExpirationDate}"
        });
        _repository.UpdateSession(session);

        return new FinalizeResponse(session.Id, session.Status.ToString(), receipt);
    }

    public RenewalReceipt GetReceipt(string receiptNumber) =>
        _repository.GetReceipt(receiptNumber) ?? throw new InvalidOperationException("Receipt not found.");

    private RenewalSession GetRequiredSession(Guid sessionId) =>
        _repository.GetSession(sessionId) ?? throw new InvalidOperationException("Session not found.");
}
