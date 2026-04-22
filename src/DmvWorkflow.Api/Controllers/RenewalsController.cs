using DmvWorkflow.Api.Models;
using DmvWorkflow.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DmvWorkflow.Api.Controllers;

[ApiController]
[Route("api/renewals")]
public class RenewalsController : ControllerBase
{
    private readonly IRenewalWorkflowService _service;

    public RenewalsController(IRenewalWorkflowService service)
    {
        _service = service;
    }

    [HttpPost("sessions")]
    public ActionResult<StartSessionResponse> StartSession([FromBody] StartSessionRequest request)
        => Ok(_service.StartSession(request));

    [HttpPost("sessions/{sessionId:guid}/lookup")]
    public ActionResult<LookupVehicleResponse> LookupVehicle(Guid sessionId, [FromBody] LookupVehicleRequest request)
        => Ok(_service.LookupVehicle(sessionId, request));

    [HttpPost("sessions/{sessionId:guid}/quote")]
    public ActionResult<QuoteResponse> CreateQuote(Guid sessionId, [FromBody] CreateQuoteRequest request)
        => Ok(_service.CreateQuote(sessionId, request));

    [HttpPost("sessions/{sessionId:guid}/payment")]
    public ActionResult<PaymentResponse> SubmitPayment(Guid sessionId, [FromBody] SubmitPaymentRequest request)
        => Ok(_service.SubmitPayment(sessionId, request));

    [HttpPost("sessions/{sessionId:guid}/finalize")]
    public ActionResult<FinalizeResponse> FinalizeRenewal(Guid sessionId)
        => Ok(_service.Finalize(sessionId));

    [HttpGet("receipts/{receiptNumber}")]
    public ActionResult<RenewalReceipt> GetReceipt(string receiptNumber)
        => Ok(_service.GetReceipt(receiptNumber));
}
