Quality Inspection Tracker

A mobile-first Quality Inspection Tracker for shop-floor supervisors to
log, monitor, and resolve fabric quality defects.

Built for the Arvind Limited -- AI & Analytics Full Stack Developer
Hiring Assignment.

1. Quick Start

Prerequisites

Install:

.NET 10 SDK

Node.js 18+ / 20+

Angular CLI

SQL Server LocalDB / SQL Server

Visual Studio 2022 or VS Code

Backend

Open the solution in Visual Studio.

Confirm the database connection in:

QualityInspectionTracker.API/appsettings.json

Example:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=QITDB;Integrated Security=True;TrustServerCertificate=True;"
  }
}

Make sure the existing QITDB database contains the required
Users and Inspections tables.

Set QualityInspectionTracker.API as the startup project.

Restore and build:

dotnet restore
dotnet build

Run the API:

dotnet run --project QualityInspectionTracker.API

Or press F5 from Visual Studio.

Swagger opens automatically when the configured launch profile has:

"launchBrowser": true,
"launchUrl": "swagger"

Typical development URLs:

https://localhost:7171/swagger
https://localhost:44388/swagger

Use the URL for the profile you are running.

Frontend

From the Angular project:

npm install
ng serve

Open:

http://localhost:4200

The Angular application should call the API using the configured API
base URL.

For local development, CORS allows:

http://localhost:4200

2. Architecture

The solution follows a lightweight Clean Architecture / Layered
Architecture approach.

┌──────────────────────────────────────────────┐
│                Angular UI                    │
│          Mobile-first frontend               │
└──────────────────────┬───────────────────────┘
                       │ HTTP / JSON
                       ▼
┌──────────────────────────────────────────────┐
│              API Layer                       │
│ Controllers / JWT / Swagger / CORS           │
└──────────────────────┬───────────────────────┘
                       │
                       ▼
┌──────────────────────────────────────────────┐
│           Application Layer                  │
│ DTOs / Interfaces / Services / Business Rules│
└──────────────────────┬───────────────────────┘
                       │
                       ▼
┌──────────────────────────────────────────────┐
│          Infrastructure Layer                │
│ EF Core / DbContext / Repository             │
└──────────────────────┬───────────────────────┘
                       │
                       ▼
┌──────────────────────────────────────────────┐
│                 QITDB                        │
│              SQL Server                      │
│                                              │
│ Users 1 ─────────────── * Inspections        │
└──────────────────────────────────────────────┘

Domain

Contains database/domain entities:

Domain
└── Entities
    ├── User.cs
    └── Inspection.cs

The domain layer has no dependency on API or Infrastructure.

Application

Contains application contracts and business logic:

Application
├── Constants
├── DTOs
├── Interfaces
└── Services

Examples:

IInspectionService

IInspectionRepository

IUserRepository

IAuthService

IAdminService

The Application layer depends only on Domain.

Infrastructure

Contains database access:

Infrastructure
├── Data
│   └── AppDbContext.cs
└── Repositories
    ├── InspectionRepository.cs
    └── UserRepository.cs

The existing SQL Server database was scaffolded using EF Core Database
First.

API

Contains HTTP endpoints and application configuration:

API
├── Controllers
│   ├── AuthController.cs
│   ├── AdminController.cs
│   ├── InspectionsController.cs
│   └── SummaryController.cs
├── Middleware
├── Program.cs
└── appsettings.json

3. Database

The application uses the existing QITDB SQL Server database.

Main tables:

Users
  |
  | 1:N
  |
Inspections

Users

Stores:

Username

Password hash

Display name

Role

Active status

Created date

Supported roles:

Admin
Supervisor

Inspections

Stores:

Inspection date

Machine/line ID

Defect type

Severity

Remarks

Status

Resolution note

Resolved date

Source

Created by user

Created date

Inspections.CreatedByUserId references Users.Id.

This allows the application to identify which supervisor logged each
inspection.

4. Authentication and Authorization

Authentication uses JWT.

Login

POST /api/auth/login

Request:

{
  "username": "supervisor",
  "password": "your-password"
}

The API:

Finds the active user.

Verifies the BCrypt password hash.

Creates a JWT.

Adds user ID, username, display name, and role to the JWT.

Returns the token to the frontend.

Protected requests use:

Authorization: Bearer <token>

Admin authorization

Supervisor creation is restricted to Admin users:

POST /api/admin/supervisors

The endpoint uses:

[Authorize(Roles = "Admin")]

A Supervisor cannot create another Supervisor.

5. Supervisor Management

An Admin can create supervisors.

POST /api/admin/supervisors

Example:

{
  "username": "supervisor01",
  "password": "StrongPassword123",
  "displayName": "Shop Floor Supervisor"
}

The API always creates:

Role = Supervisor
IsActive = true

The frontend does not control the role.

List supervisors:

GET /api/admin/supervisors

6. Inspection APIs

Create inspection

POST /api/inspections

Example:

{
  "inspectionDate": "2026-08-21",
  "machineLineId": "LINE-05",
  "defectType": "WeaveDefect",
  "severity": "Critical",
  "remarks": "Uneven weaving detected"
}

The backend automatically sets:

Status = Open
Source = manual
CreatedAt = UTC time
CreatedByUserId = logged-in user

CreatedByUserId is obtained from the JWT and is never trusted from the
frontend.

Get inspections

GET /api/inspections

Filtering examples:

GET /api/inspections?severity=Critical
GET /api/inspections?status=Open
GET /api/inspections?fromDate=2026-08-01&toDate=2026-08-21

Sorting example:

GET /api/inspections?sortBy=severity&sortDescending=true

Get by ID

GET /api/inspections/{id}

Resolve inspection

PUT /api/inspections/{id}/resolve

Request:

{
  "resolutionNote": "Machine adjusted and fabric re-inspected successfully."
}

The backend automatically sets:

Status = Resolved
ResolvedAt = UTC time

A resolution note is mandatory.

7. Summary API

GET /api/summary

Returns Open and Resolved inspection counts grouped by severity.

Example:

{
  "critical": {
    "open": 3,
    "resolved": 5
  },
  "major": {
    "open": 8,
    "resolved": 12
  },
  "minor": {
    "open": 15,
    "resolved": 20
  }
}

8. Swagger and JWT

Swagger is enabled for development.

Open:

https://localhost:<port>/swagger

The Swagger UI contains a Authorize button.

Call POST /api/auth/login.

Copy the returned token.

Click Authorize.

Paste the JWT token.

Test protected APIs.

The token is then automatically sent as:

Authorization: Bearer <token>

9. CORS

During local development, the API allows the Angular development server:

http://localhost:4200

This is configured in Program.cs.

For production, the allowed origin should be changed to the actual
deployed frontend URL instead of allowing arbitrary origins.

10. Important Project Dependencies

Dependency direction:

Domain
  ↑
Application
  ↑
Infrastructure
  ↑
API

More precisely:

Application → Domain
Infrastructure → Application + Domain
API → Application + Infrastructure

The Domain and Application layers do not depend on Infrastructure.

This keeps database implementation details outside the
business/application layer.

11. Why Repository + Service Pattern?

Repository

The repository is responsible for database operations:

InspectionRepository
UserRepository

It handles:

EF Core queries

Filtering

Sorting

Insert/update operations

Database persistence

Service

The service contains business logic:

InspectionService
AuthService
AdminService

For example, resolving an inspection requires:

Find inspection
      ↓
Check it exists
      ↓
Check it is not already resolved
      ↓
Validate resolution note
      ↓
Set status = Resolved
      ↓
Set ResolvedAt
      ↓
Save

This keeps controllers thin and makes the business logic easier to test
and maintain.

12. Database-First Decision

The database was already provided/created, so EF Core Database First /
Scaffold-DbContext was used rather than Code First migrations.

The scaffolded AppDbContext maps the existing:

Users
Inspections

tables and their existing relationship/indexes/defaults.

This avoids modifying an existing database schema unnecessarily.

13. Security Decisions

Passwords are stored as BCrypt hashes, never plain text.

JWT is used for API authentication.

User ID and role are stored in JWT claims.

CreatedByUserId is obtained from the authenticated user rather
than from the client request.

Supervisor creation is restricted to Admin users.

Password hashes are never returned in API responses.

Database connection strings and JWT secrets should not contain
production secrets in source control.

14. What I Would Do Differently With More Time

With more time, I would consider:

Add refresh tokens and stronger token/session management.

Add pagination for large inspection datasets.

Add automated unit and integration tests.

Add centralized validation using FluentValidation.

Add structured logging and correlation IDs.

Add offline inspection capture and synchronization for shop-floor
connectivity issues.

Implement the optional SAP webhook with idempotency protection.

Add role-based administration for activating/deactivating
supervisors.

Add Docker support for consistent local setup.

Add CI/CD validation and automated tests.

15. Quick Troubleshooting

API cannot connect to database

Check:

appsettings.json
ConnectionStrings:DefaultConnection

and verify that QITDB exists.

Angular gets CORS error

Make sure the API allows:

http://localhost:4200

and that Angular is calling the same API port shown by the running .NET
profile.

Swagger does not open automatically

Check:

Properties/launchSettings.json

contains:

"launchBrowser": true,
"launchUrl": "swagger"

JWT returns 401

Check:

Token is not expired.

JWT key is the same between token creation and validation.

Issuer matches.

Audience matches.

Request contains:

Authorization: Bearer <token>

Admin endpoint returns 403

The logged-in user must have:

Role = Admin

A Supervisor token cannot call Admin-only endpoints.

16. Assignment Feature Coverage

Requirement                 Status

Log inspection              Implemented
Date                        Implemented
Machine/Line ID             Implemented
Defect type                 Implemented
Severity                    Implemented
Remarks                     Implemented
Inspection list             Implemented
Filtering                   Implemented
Sorting                     Implemented
Date range                  Implemented
Resolve inspection          Implemented
Mandatory resolution note   Implemented
Open/Resolved summary       Implemented
Mobile-first Angular UI     Implemented
JWT authentication          Implemented
Admin creates supervisors   Implemented
Supervisor audit trail      Implemented
Swagger                     Implemented
CORS                        Implemented
SAP webhook                 Optional
Offline synchronization     Optional

17. Recommended Demo Flow

For a reviewer/demo:

1. Start SQL Server / LocalDB
        ↓
2. Run .NET API
        ↓
3. Swagger opens
        ↓
4. Login as Admin
        ↓
5. Create Supervisor
        ↓
6. Login as Supervisor
        ↓
7. Create inspection
        ↓
8. Verify CreatedBy supervisor
        ↓
9. Filter inspections
        ↓
10. Resolve an inspection
        ↓
11. Open Summary
        ↓
12. Verify Open/Resolved counts

This demonstrates the complete business flow from user management to
inspection audit and resolution.
