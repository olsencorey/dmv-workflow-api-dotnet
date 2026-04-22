# DMV Workflow API (.NET 8)

A portfolio-ready REST API built with C# and ASP.NET Core that models a DMV-style vehicle registration renewal workflow. The project is inspired by modern self-service motor vehicle experiences, where a resident can start a renewal session, look up a vehicle, calculate fees, submit payment, and receive a receipt in one streamlined flow.

## Why I built this

I wanted a backend portfolio project that felt more realistic than basic CRUD. DMV and government transaction systems are a good example of software that has to balance clean APIs, workflow state management, validation rules, and auditability.

This project is designed to demonstrate:
- REST API design in ASP.NET Core
- layered backend architecture
- workflow/state-based business logic
- validation and error handling patterns
- domain modeling for transaction-heavy systems
- a clean foundation for future database and cloud enhancements

## Workflow overview

This API models a vehicle registration renewal flow with these steps:

1. Start a renewal session.
2. Look up a vehicle by registration notice number or by plate number + VIN suffix.
3. Validate the vehicle for renewal eligibility.
4. Generate a quote for a 12- or 24-month renewal.
5. Submit payment.
6. Finalize the renewal.
7. Return a receipt with the updated expiration date.

This is the same kind of backend workflow you would expect behind a kiosk, web portal, or clerk-assisted DMV channel.

## Architecture

The project uses a simple layered structure:

- **Controllers**: define HTTP endpoints and keep request handling thin
- **Services**: contain renewal workflow and business rules
- **Repositories**: abstract persistence behind an interface
- **Models**: represent the domain entities, request DTOs, and response DTOs

Current persistence is in-memory to keep the project lightweight and easy to run, but the repository pattern makes it straightforward to swap in EF Core with PostgreSQL or SQL Server later.

## Project structure

```text
src/
  DmvWorkflow.Api/
    Controllers/
      RenewalsController.cs
    Models/
      Enums.cs
      VehicleRecord.cs
      OwnerRecord.cs
      AuditEvent.cs
      RenewalSession.cs
      RenewalQuote.cs
      PaymentTransaction.cs
      RenewalReceipt.cs
      Requests.cs
      Responses.cs
    Repositories/
      IDmvRepository.cs
      InMemoryDmvRepository.cs
    Services/
      IRenewalWorkflowService.cs
      RenewalWorkflowService.cs
    Program.cs
    appsettings.json
```

## Endpoints

| Method | Route | Description |
|---|---|---|
| POST | `/api/renewals/sessions` | Start a renewal session |
| POST | `/api/renewals/sessions/{sessionId}/lookup` | Look up a vehicle |
| POST | `/api/renewals/sessions/{sessionId}/quote` | Create a renewal quote |
| POST | `/api/renewals/sessions/{sessionId}/payment` | Submit payment |
| POST | `/api/renewals/sessions/{sessionId}/finalize` | Finalize the renewal |
| GET | `/api/renewals/receipts/{receiptNumber}` | Retrieve the receipt |

## Sample API usage

### 1. Start session

**Request**

```json
{
  "channel": "Kiosk",
  "kioskId": "AZ-PHX-001"
}
```

**Response**

```json
{
  "sessionId": "3f68f373-48eb-4c8b-b653-ef960df4e9fd",
  "status": "Started"
}
```

### 2. Look up vehicle

**Request**

```json
{
  "noticeNumber": "RN-2026-0001"
}
```

**Response**

```json
{
  "sessionId": "3f68f373-48eb-4c8b-b653-ef960df4e9fd",
  "status": "VehicleMatched",
  "vehicle": {
    "id": "f4c14f92-3e74-44c7-b615-dfdedf6f962f",
    "plateNumber": "AZX4219",
    "vinLast6": "941203",
    "noticeNumber": "RN-2026-0001",
    "make": "Toyota",
    "model": "Camry",
    "year": 2021,
    "registrationExpiresOn": "2026-05-31",
    "emissionsHold": false,
    "insuranceVerified": true,
    "ownerId": "9c56c16f-80d6-47dc-a5c5-a6953552cb7d"
  },
  "owner": {
    "id": "9c56c16f-80d6-47dc-a5c5-a6953552cb7d",
    "fullName": "Jordan Ramirez",
    "addressLine1": "1450 Desert Willow Ave",
    "city": "Phoenix",
    "state": "AZ",
    "postalCode": "85004"
  }
}
```

### 3. Create quote

**Request**

```json
{
  "months": 12,
  "deliveryMethod": "PrintAtKiosk"
}
```

**Response**

```json
{
  "sessionId": "3f68f373-48eb-4c8b-b653-ef960df4e9fd",
  "status": "Quoted",
  "quote": {
    "id": "bdf92524-c87f-4647-937c-30c47f2b424f",
    "sessionId": "3f68f373-48eb-4c8b-b653-ef960df4e9fd",
    "months": 12,
    "registrationFee": 92.0,
    "countyFee": 22.5,
    "processingFee": 4.0,
    "total": 118.5,
    "deliveryMethod": "PrintAtKiosk",
    "expiresAtUtc": "2026-04-22T03:00:00Z"
  }
}
```

### 4. Submit payment

**Request**

```json
{
  "paymentMethod": "Card",
  "amount": 118.5,
  "last4": "4242"
}
```

**Response**

```json
{
  "sessionId": "3f68f373-48eb-4c8b-b653-ef960df4e9fd",
  "status": "PaymentAuthorized",
  "payment": {
    "id": "85fd2c65-23e5-42ba-a72e-6b9cb986b471",
    "sessionId": "3f68f373-48eb-4c8b-b653-ef960df4e9fd",
    "paymentMethod": "Card",
    "amount": 118.5,
    "last4": "4242",
    "authorizationCode": "AUTH-483920",
    "authorizedAtUtc": "2026-04-22T03:05:00Z"
  }
}
```

### 5. Finalize renewal

**Response**

```json
{
  "sessionId": "3f68f373-48eb-4c8b-b653-ef960df4e9fd",
  "status": "Completed",
  "receipt": {
    "receiptNumber": "RCPT-20260422-4821",
    "sessionId": "3f68f373-48eb-4c8b-b653-ef960df4e9fd",
    "plateNumber": "AZX4219",
    "newExpirationDate": "2027-05-31",
    "amountPaid": 118.5,
    "issuedAtUtc": "2026-04-22T03:06:00Z",
    "deliveryMethod": "PrintAtKiosk"
  }
}
```

## Local setup

### Prerequisites
- .NET 8 SDK
- Optional: Visual Studio 2022 or VS Code

### Run locally

```bash
dotnet restore
cd src/DmvWorkflow.Api
dotnet run
```

Then open Swagger in the browser at:

```text
https://localhost:xxxx/swagger
```

## Current design decisions

- Uses an in-memory repository for quick setup and easy demoability
- Keeps controllers thin and business logic in the service layer
- Uses DTO-style request and response records for clean API contracts
- Seeds one sample owner and one sample vehicle for testing
- Models an auditable workflow with clear state transitions

## Future enhancements

Here are the next upgrades I would make to productionize the project further:

- Replace in-memory persistence with **EF Core + PostgreSQL**
- Add **global exception handling middleware**
- Add **FluentValidation** or a similar validation layer
- Add **integration tests** with `WebApplicationFactory`
- Add **authentication and authorization**
- Add **idempotency keys** for payment/finalization endpoints
- Add **Docker** support
- Add **GitHub Actions CI** workflow for restore/build/test
- Add configuration-driven rules to support multiple states or DMV programs

## Interview talking points

This is the kind of project I would use in an interview to discuss:

- how I design RESTful workflows beyond simple CRUD
- how I separate controller, service, and repository concerns
- how I model domain entities and state transitions
- how I think about auditability and validation in regulated systems
- how I would evolve a simple API into a cloud-ready production service

## License

This project is for portfolio and learning purposes.
