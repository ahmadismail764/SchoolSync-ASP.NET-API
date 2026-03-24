# SchoolSync API - Code Review & Action Items

## 🌟 What Went Well (Strengths to Maintain)

- **Clean Architecture:** Excellent separation of concerns across API, App, Domain, and Infra layers. Thin controllers delegating to services.
- **Global Exception Handling:** Great use of middleware (`ExceptionHandlingMiddleware.cs`) to map exceptions (e.g., `KeyNotFoundException`) directly to HTTP status codes.
- **DTOs & AutoMapper:** Proper protection of the database schema and prevention of over-posting attacks.
- **Soft Deletes:** Enterprise-standard practice implemented effectively via the `IsDeleted` flag across repositories.

---

## 🚨 Critical Architectural Fixes (High Priority)

### 1. Stop Storing Files as `byte[]` in the Database

- **Issue:** `Material.cs` and `School.cs` store PDFs/images as `byte[]`. This rapidly bloats the database, degrades query performance, consumes massive RAM, and makes backups expensive.
- **Action Item:** \* Refactor to save physical files to the local file system (e.g., `wwwroot/uploads`) or a cloud storage provider (e.g., Azure Blob Storage, AWS S3).
  - Update the database schema to store only the **file path or URL** as a `string`.

### 2. Fix the In-Memory JWT Blacklist

- **Issue:** `TokenService.cs` uses a `static HashSet<string>` for revoked tokens. If the application crashes, restarts, or scales to multiple servers, the RAM is cleared, and all "revoked" tokens become valid again.
- **Action Item:** Store revoked tokens (or token revocation timestamps) in a persistent data store like Redis, or directly in a SQL database table.

---

## 🤔 Refactoring Opportunities (Technical Debt)

### 1. Move Away from the Generic Repository Anti-Pattern

- **Issue:** `GenericRepo.GetIncludes()` uses Reflection to blindly `.Include()` all navigation properties. Querying a `School` might pull all `Users`, `Subjects`, and `SchoolYears`, leading to **Cartesian Explosions** (fetching gigabytes of redundant data).
- **Action Item:** Rely on specific repositories (e.g., `SchoolRepo`). Only call `.Include()` for exactly the data you need for a specific query.

### 2. Optimize Bulk Updates & Deletes

- **Issue:** `GenericRepo.UpdateRangeWhereAsync` fetches all records into RAM, uses reflection to update them, and then saves. Doing this on 10,000 records will crash the app.
- **Action Item:** Use EF Core 8's `ExecuteUpdateAsync()` and `ExecuteDeleteAsync()` to translate commands directly to SQL without pulling data into memory.

```csharp
// Refactor to use EF Core 8 bulk updates:
await dbSet.Where(predicate)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsActive, false));
```

### 3. Clean Up Validation with FluentValidation

- **Issue:** `ValidateCreateAsync` methods in your services are filled with manual `if/throw` blocks, muddying the business logic.
- **Action Item:** Integrate the **FluentValidation** library. Create dedicated validator classes for your DTOs to catch validation errors _before_ they hit your controllers or services.

### 4. Standardize RESTful Routing

- **Issue:** Inconsistent route naming patterns.
- **Action Item:** Ensure all routes consistently use plural nouns (e.g., rename `api/lesson` to `api/lessons`).
