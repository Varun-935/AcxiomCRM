# AcxiomCRM

AcxiomCRM is a CRM web application for tracking customers, leads, sales opportunities, follow-ups, and audit activity.

## Built with
- ASP.NET Core MVC (.NET 8)
- ASP.NET Core Identity for authentication and roles
- Entity Framework Core
- SQLite for a quick local setup
- Bootstrap 5 for responsive styling

## Run locally
1. Install the .NET 8 SDK.
2. Open a terminal in this folder.
3. Run `dotnet restore`.
4. Run `dotnet run`.
5. Open the local URL printed in the terminal.

A development administrator is created on first launch:
- Email: `admin@acxiomcrm.local`
- Password: `Admin@12345`

Change this development password before using the application outside a local demo. The database is stored in `acxiomcrm.db` and created automatically on first launch.

## Included modules
- Identity login, registration and logout
- Customer create, search, edit and deactivate
- Lead create, search, edit and delete
- Opportunity pipeline with stage updates and weighted pipeline value
- Follow-up scheduling and completion
- Dashboard KPIs
- Admin-only audit log
- Data annotations and server-side business validation

## Scope notes
This is a starter implementation based on the supplied AcxiomCRM functional specification. Review and test all security, authorization, reporting, and validation requirements before describing the application as production-ready. Role creation is included; role-specific access policies can be extended as the project grows.
