# SchoolSync ASP.NET Core API

SchoolSync is a robust, modular, and extensible ASP.NET Core Web API designed for comprehensive school and academic management. It supports organizations, schools, users, roles, subjects, lessons, materials, enrollments, school years, and terms. The system is built with clean architecture principles, strong validation, and a focus on maintainability and scalability.

---

## Table of Contents

- [Project Structure](#project-structure)
- [Features](#features)
- [Domain Model](#domain-model)
- [API Overview](#api-overview)
- [Validation & Business Logic](#validation--business-logic)
- [File Uploads & Downloads](#file-uploads--downloads)
- [Authentication & Authorization](#authentication--authorization)
- [Getting Started](#getting-started)
- [Development & Architecture](#development--architecture)
- [Troubleshooting](#troubleshooting)
- [License](#license)

---

## Project Structure

- **SchoolSync.API** — Main API project (controllers, startup, configuration)
- **SchoolSync.Infra** — Infrastructure layer (EF Core DbContext, migrations, repositories)
- **SchoolSync.Domain** — Domain entities and interfaces (core business models)
- **SchoolSync.App** — Application services (business logic, validation, service layer)

---

## Features

- **Organization, School, User, Role, Subject, Lesson, Material, Enrollment, SchoolYear, Term management**
- **File upload/download** for lesson materials and school/organization logos
- **JWT-based authentication** and role-based authorization
- **Entity validation** with strict separation for create/update logic
- **AutoMapper** for DTO mapping
- **Minimal, DRY controllers** with all business logic in services
- **Extensible, testable architecture** (Domain-Driven Design, Clean Architecture)

---

## Domain Model

### Core Entities

- **Organization**: Top-level entity, can have multiple schools. Has a nullable logo (byte[]).
- **School**: Belongs to an organization. Has users, subjects, school years, and a nullable logo (byte[]).
- **User**: Represents students, teachers, admins. Linked to a school and a role. Has authentication credentials.
- **Role**: Defines user permissions (e.g., Student, Teacher, Admin).
- **Subject**: Academic subject, linked to a school and a teacher.
- **Lesson**: Linked to a subject. Can have multiple materials.
- **Material**: File (PDF, video, etc.) linked to a lesson. Stored as byte[].
- **Enrollment**: Links students to subjects and terms.
- **SchoolYear**: Academic year for a school. Has multiple terms.
- **Term**: Academic term (semester/quarter), linked to a school year.

### Relationships

- Organization 1:M School
- School 1:M User, 1:M Subject, 1:M SchoolYear
- User 1:M Enrollment (as Student), 1:M Subject (as Teacher)
- Subject 1:M Lesson
- Lesson 1:M Material
- SchoolYear 1:M Term
- Enrollment M:1 Subject, M:1 Term

---

## API Overview

### Main Endpoints

- `/api/organizations` — CRUD for organizations, logo upload
- `/api/schools` — CRUD for schools, logo upload
- `/api/users` — CRUD for users, authentication
- `/api/roles` — CRUD for user roles
- `/api/subjects` — CRUD for subjects
- `/api/lessons` — CRUD for lessons
- `/api/materials` — Upload/download lesson materials
- `/api/enrollments` — Manage student enrollments
- `/api/schoolyears` — Manage school years
- `/api/terms` — Manage academic terms

### File Uploads

- Logos and materials are uploaded via dedicated endpoints (e.g., `/schools/{id}/upload-logo`)
- All file data is stored as `byte[]` in the database (nullable for logos)

### Authentication

- JWT-based authentication
- Endpoints for login and token issuance

### Example Request: Create a School

```json
{
  "name": "Springfield High",
  "address": "123 Main St",
  "phoneNumber": "555-1234",
  "email": "info@springfieldhigh.edu",
  "organizationId": 1
}
```

---

## Validation & Business Logic

- All entities are validated on create/update
- Uniqueness and required fields are enforced (e.g., usernames, emails, subject codes)
- Validation logic is separated for create and update operations (e.g., uniqueness only on create)
- Cross-entity validation (e.g., teacher must belong to the same school as the subject)

---

## File Uploads & Downloads

- **Logos**: Uploaded after entity creation via `/organizations/{id}/upload-logo` or `/schools/{id}/upload-logo`
- **Materials**: Uploaded via `/materials/upload` endpoint, linked to lessons
- All files are stored as `byte[]` in the database; logos are nullable

---

## Authentication & Authorization

- **JWT-based authentication**: Secure endpoints, issue tokens on login
- **Role-based authorization**: Restrict access to endpoints based on user roles

---

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

---

## Development & Architecture

- **Clean Architecture**: Separation of concerns between API, application, domain, and infrastructure layers
- **AutoMapper**: Used for mapping between DTOs and entities
- **Minimal Controllers**: All business logic is in services, controllers are thin
- **Validation**: All validation is in services, not controllers
- **Extensibility**: Add new entities/services by following the existing patterns

---

## Troubleshooting

- **EF Core Include Errors**: Only use `.Include()` for navigation properties, not scalar fields like `Logo`
- **Validation Errors**: Check that all required fields are provided in your requests
- **Dependency Injection**: Ensure all services are registered in `ServiceCollectionExtensions`
- **Migrations**: Always use `SchoolSync.API` as the startup project for migrations

---

## License

MIT
