# DMV Workflow API (.NET 8)

Portfolio-ready REST API in C#/.NET 8 for a DMV-style registration renewal workflow inspired by Arizona MVD self-service and kiosk flows.

## Features
- Start renewal sessions from kiosk, web, or clerk channel.
- Lookup vehicle by notice number or plate + VIN suffix.
- Quote 12- or 24-month renewals.
- Authorize payment and finalize renewal.
- Generate receipt data and maintain an audit trail.

## Endpoints
- POST /api/renewals/sessions
- POST /api/renewals/sessions/{sessionId}/lookup
- POST /api/renewals/sessions/{sessionId}/quote
- POST /api/renewals/sessions/{sessionId}/payment
- POST /api/renewals/sessions/{sessionId}/finalize
- GET /api/renewals/receipts/{receiptNumber}
