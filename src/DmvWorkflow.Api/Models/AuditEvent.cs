namespace DmvWorkflow.Api.Models;

public class AuditEvent
{
    public DateTimeOffset OccurredAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public string EventType { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
}
