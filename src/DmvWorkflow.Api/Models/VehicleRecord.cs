namespace DmvWorkflow.Api.Models;

public class VehicleRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PlateNumber { get; set; } = string.Empty;
    public string VinLast6 { get; set; } = string.Empty;
    public string NoticeNumber { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public DateOnly RegistrationExpiresOn { get; set; }
    public bool EmissionsHold { get; set; }
    public bool InsuranceVerified { get; set; }
    public Guid OwnerId { get; set; }
}
