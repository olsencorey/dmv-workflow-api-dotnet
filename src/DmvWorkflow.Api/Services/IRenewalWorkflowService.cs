using DmvWorkflow.Api.Models;

namespace DmvWorkflow.Api.Services;

public interface IRenewalWorkflowService
{
    StartSessionResponse StartSession(StartSessionRequest request);
    LookupVehicleResponse LookupVehicle(Guid sessionId, LookupVehicleRequest request);
    QuoteResponse CreateQuote(Guid sessionId, CreateQuoteRequest request);
    PaymentResponse SubmitPayment(Guid sessionId, SubmitPaymentRequest request);
    FinalizeResponse Finalize(Guid sessionId);
    RenewalReceipt GetReceipt(string receiptNumber);
}
