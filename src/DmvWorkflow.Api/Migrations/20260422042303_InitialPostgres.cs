using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DmvWorkflow.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    AddressLine1 = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Last4 = table.Column<string>(type: "text", nullable: true),
                    AuthorizationCode = table.Column<string>(type: "text", nullable: false),
                    AuthorizedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RenewalQuotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Months = table.Column<int>(type: "integer", nullable: false),
                    RegistrationFee = table.Column<decimal>(type: "numeric", nullable: false),
                    CountyFee = table.Column<decimal>(type: "numeric", nullable: false),
                    ProcessingFee = table.Column<decimal>(type: "numeric", nullable: false),
                    DeliveryMethod = table.Column<int>(type: "integer", nullable: false),
                    ExpiresAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenewalQuotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RenewalReceipts",
                columns: table => new
                {
                    ReceiptNumber = table.Column<string>(type: "text", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlateNumber = table.Column<string>(type: "text", nullable: false),
                    NewExpirationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "numeric", nullable: false),
                    IssuedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeliveryMethod = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenewalReceipts", x => x.ReceiptNumber);
                });

            migrationBuilder.CreateTable(
                name: "RenewalSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Channel = table.Column<int>(type: "integer", nullable: false),
                    KioskId = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: true),
                    QuoteId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReceiptNumber = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenewalSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlateNumber = table.Column<string>(type: "text", nullable: false),
                    VinLast6 = table.Column<string>(type: "text", nullable: false),
                    NoticeNumber = table.Column<string>(type: "text", nullable: false),
                    Make = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    RegistrationExpiresOn = table.Column<DateOnly>(type: "date", nullable: false),
                    EmissionsHold = table.Column<bool>(type: "boolean", nullable: false),
                    InsuranceVerified = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Owners",
                columns: new[] { "Id", "AddressLine1", "City", "FullName", "PostalCode", "State" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "1450 Desert Willow Ave", "Phoenix", "Jordan Ramirez", "85004", "AZ" });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "EmissionsHold", "InsuranceVerified", "Make", "Model", "NoticeNumber", "OwnerId", "PlateNumber", "RegistrationExpiresOn", "VinLast6", "Year" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), false, true, "Toyota", "Camry", "RN-2026-0001", new Guid("11111111-1111-1111-1111-111111111111"), "AZX4219", new DateOnly(2026, 5, 31), "941203", 2021 });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_NoticeNumber",
                table: "Vehicles",
                column: "NoticeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_PlateNumber_VinLast6",
                table: "Vehicles",
                columns: new[] { "PlateNumber", "VinLast6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.DropTable(
                name: "RenewalQuotes");

            migrationBuilder.DropTable(
                name: "RenewalReceipts");

            migrationBuilder.DropTable(
                name: "RenewalSessions");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
