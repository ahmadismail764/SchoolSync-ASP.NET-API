# SchoolSync ASP.NET Core API

SchoolSync is a modular ASP.NET Core Web API for managing schools, organizations, students, and academic data. It uses Entity Framework Core for data access and SQL Server as the default database.

## Project Structure

- `SchoolSync.API` — Main API project (startup project)
- `SchoolSync.Infra` — Infrastructure layer (EF Core DbContext, migrations, repositories)
- `SchoolSync.Domain` — Domain entities and interfaces
- `SchoolSync.App` — Application services

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)

### Configuration

1. Update the connection string in `SchoolSync.API/appsettings.Development.json` if needed:

   ```json
   "DevDB": "Server=localhost;Database=SchoolSync;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;"
   ```

### Database Setup

1. Open a terminal in the solution root directory.
2. Run the following commands:

   ```powershell
   dotnet tool install --global dotnet-ef # (if not already installed)
   dotnet ef migrations add InitialCreate --project SchoolSync.Infra --startup-project SchoolSync.API
   dotnet ef database update --project SchoolSync.Infra --startup-project SchoolSync.API
   ```

### Running the API

1. Set `SchoolSync.API` as the startup project.
2. Run the API:

   ```powershell
   dotnet run --project SchoolSync.API
   ```

3. The API will be available at `https://localhost:5001` or `http://localhost:5000` by default.

## Notes

- Migrations and database updates should always use `SchoolSync.API` as the startup project.
- Warnings about decimal precision can be resolved by configuring precision in your entity models.

## License

MIT
