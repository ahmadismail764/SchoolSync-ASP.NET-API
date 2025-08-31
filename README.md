# SchoolSync ASP.NET Core API

SchoolSync is a production-grade, modular, and extensible ASP.NET Core Web API for managing the full lifecycle of academic organizations. It is engineered with a strong focus on maintainability, testability, and scalability, using modern software engineering principles such as Clean Architecture, Domain-Driven Design (DDD), and SOLID. The system is designed to be realistic for real-world deployments in schools, universities, or educational platforms.

---

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

## Project Structure & Engineering Principles

- **SchoolSync.API** — API layer. Contains controllers, dependency injection setup, and configuration. Controllers are kept minimal, delegating all business logic to services.
- **SchoolSync.App** — Application layer. Implements all business logic, validation, and orchestration. Follows the Service Layer pattern and enforces separation of concerns.
- **SchoolSync.Domain** — Domain layer. Defines core business entities, value objects, and repository/service interfaces. Models are persistence-agnostic and encapsulate business rules.
- **SchoolSync.Infra** — Infrastructure layer. Implements data access (EF Core), repositories, and database migrations. All infrastructure dependencies are injected via interfaces.

**Key Engineering Principles:**

- **Clean Architecture**: Each layer has clear responsibilities and dependencies only point inward.
- **Domain-Driven Design (DDD)**: The domain model is at the center, with rich validation and business rules.
- **SOLID Principles**: Code is modular, extensible, and easy to test or refactor.
- **DRY & KISS**: Controllers are thin, logic is centralized, and code is easy to follow.
- **Separation of Concerns**: Validation, mapping, and persistence are all handled in their own layers.

---

---

## Features

- **Comprehensive Academic Management**: Manage organizations, schools, users, roles, subjects, lessons, materials, enrollments, school years, and terms.
- **Robust File Handling**: Upload/download lesson materials and school/organization logos. All files are stored as `byte[]` in the database, with dedicated endpoints for uploads.
- **Secure Authentication & Authorization**: JWT-based authentication, role-based access control, and secure endpoints.
- **Rich Validation**: All entities are validated on create/update, with clear separation of uniqueness and business rules.
- **AutoMapper Integration**: DTOs and entities are mapped automatically, reducing boilerplate and improving maintainability.
- **Minimal, DRY Controllers**: Controllers only handle HTTP concerns; all business logic is in services.
- **Extensible & Testable**: The architecture supports easy addition of new features, and all layers are unit-testable.

---

---

## Domain Model & Relationships

### Core Entities

- **Organization**: Top-level entity, can have multiple schools. Has a nullable logo (byte[]).
- **School**: Belongs to an organization. Has users, subjects, school years, and a nullable logo (byte[]).
- **User**: Represents students, teachers, admins. Linked to a school and a role. Has authentication credentials and role-based permissions.
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

### Example: Creating a School (Realistic Request)

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

---

## Validation & Business Logic

- **Centralized Validation**: All validation is performed in the service layer, not in controllers. This ensures consistency and reusability.
- **Operation-Specific Rules**: Uniqueness and required fields are enforced on create, while update validation is more permissive.
- **Cross-Entity Checks**: Business rules such as "teacher must belong to the same school as the subject" are enforced in services.
- **Fail Fast**: Invalid data is rejected early, with clear error messages and parameter names.

---

---

## File Uploads & Downloads

- **Logo Uploads**: Logos are uploaded after entity creation, ensuring the main entity can be created without a file. Endpoints: `/organizations/{id}/upload-logo`, `/schools/{id}/upload-logo`.
- **Material Uploads**: Lesson materials (PDFs, videos, etc.) are uploaded via `/materials/upload` and linked to lessons. File validation and processing are handled in a dedicated handler.
- **Database Storage**: All files are stored as `byte[]` in the database. Logos are nullable, supporting a two-step creation/upload process.

---

---

## Authentication & Authorization

- **JWT Authentication**: All endpoints are secured with JWT tokens. Users authenticate and receive a token for subsequent requests.
- **Role-Based Access Control**: Endpoints are protected by user roles (e.g., only admins can create schools or organizations).

---

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

---

## Development & Architecture

- **Clean Architecture**: Each layer has a single responsibility and clear boundaries. Dependencies always point inward.
- **AutoMapper**: Used for mapping between DTOs and entities, reducing boilerplate and improving maintainability.
- **Minimal Controllers**: Controllers are thin and only handle HTTP concerns. All business logic, validation, and orchestration are in services.
- **Centralized Validation**: All validation is performed in the service layer, ensuring consistency and testability.
- **Extensibility**: New entities, services, or features can be added by following the established patterns and interfaces.
- **Testability**: All layers are unit-testable, and business logic is decoupled from infrastructure.

---

---

## Troubleshooting & Best Practices

- **EF Core Include Errors**: Only use `.Include()` for navigation properties, not scalar fields like `Logo`.
- **Validation Errors**: Ensure all required fields are provided in your requests. Error messages are descriptive and parameterized.
- **Dependency Injection**: All services must be registered in `ServiceCollectionExtensions` for proper resolution.
- **Migrations**: Always use `SchoolSync.API` as the startup project for migrations to ensure correct context and configuration.
- **Separation of Concerns**: Never put business logic in controllers or repositories—always use services.

---

---

## License

MIT
