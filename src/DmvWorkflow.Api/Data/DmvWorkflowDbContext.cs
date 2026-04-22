using DmvWorkflow.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DmvWorkflow.Api.Data;

public class DmvWorkflowDbContext : DbContext
{
    public DmvWorkflowDbContext(DbContextOptions<DmvWorkflowDbContext> options) : base(options)
    {
    }

    public DbSet<VehicleRecord> Vehicles => Set<VehicleRecord>();
    public DbSet<OwnerRecord> Owners => Set<OwnerRecord>();
    public DbSet<RenewalSession> RenewalSessions => Set<RenewalSession>();
    public DbSet<RenewalQuote> RenewalQuotes => Set<RenewalQuote>();
    public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
    public DbSet<RenewalReceipt> RenewalReceipts => Set<RenewalReceipt>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleRecord>().HasKey(v => v.Id);
        modelBuilder.Entity<OwnerRecord>().HasKey(o => o.Id);
        modelBuilder.Entity<RenewalSession>().HasKey(s => s.Id);
        modelBuilder.Entity<RenewalQuote>().HasKey(q => q.Id);
        modelBuilder.Entity<PaymentTransaction>().HasKey(p => p.Id);
        modelBuilder.Entity<RenewalReceipt>().HasKey(r => r.ReceiptNumber);

        modelBuilder.Entity<RenewalSession>().Ignore(s => s.AuditTrail);

        modelBuilder.Entity<VehicleRecord>()
            .HasIndex(v => v.NoticeNumber)
            .IsUnique();

        modelBuilder.Entity<VehicleRecord>()
            .HasIndex(v => new { v.PlateNumber, v.VinLast6 });

        modelBuilder.Entity<OwnerRecord>().HasData(
            new OwnerRecord
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                FullName = "Jordan Ramirez",
                AddressLine1 = "1450 Desert Willow Ave",
                City = "Phoenix",
                State = "AZ",
                PostalCode = "85004"
            }
        );

        modelBuilder.Entity<VehicleRecord>().HasData(
            new VehicleRecord
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                OwnerId = new Guid("11111111-1111-1111-1111-111111111111"),
                PlateNumber = "AZX4219",
                VinLast6 = "941203",
                NoticeNumber = "RN-2026-0001",
                Make = "Toyota",
                Model = "Camry",
                Year = 2021,
                RegistrationExpiresOn = new DateOnly(2026, 5, 31),
                EmissionsHold = false,
                InsuranceVerified = true
            }
        );
    }
}
