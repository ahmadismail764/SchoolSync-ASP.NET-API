# SchoolSync Requirements Document

## 1. Introduction

SchoolSync is a comprehensive school management system designed to streamline administrative and academic processes. It provides a centralized platform for managing organizations, schools, users, subjects, enrollments, and educational materials. The system is built with a clear separation of concerns, with a RESTful API for a backend and a user-friendly interface for the frontend. This document outlines the requirements for developing a similar application using Java Spring Boot.

## 2. User Roles and Permissions

The system defines three user roles with distinct permissions:

*   **Admin:** Has full access to all system functionalities, including managing organizations, schools, users, and all other data.
*   **Teacher:** Can manage subjects, lessons, materials, and enrollments. Teachers can also view student information.
*   **Student:** Can view their enrolled subjects, lessons, and materials.

## 3. Functional Requirements

### 3.1. Authentication Module

*   **User Registration:** `POST /api/auth/register` - Allows new users to register. The system sends a verification email with a temporary password.
*   **User Login:** `POST /api/auth/login` - Authenticates users and returns a JWT token.
*   **Set Password:** `POST /api/auth/set-password` - Allows users to set a new password after verifying the temporary one.

### 3.2. Organization Module

*   **Create Organization:** `POST /api/organizations`
*   **Get All Organizations:** `GET /api/organizations`
*   **Get Organization by ID:** `GET /api/organizations/{id}`
*   **Update Organization:** `PUT /api/organizations/{id}`
*   **Delete Organization:** `DELETE /api/organizations/{id}`
*   **Upload Organization Logo:** `POST /api/organizations/{id}/upload-logo`

### 3.3. School Module

*   **Create School:** `POST /api/schools`
*   **Get All Schools:** `GET /api/schools`
*   **Get School by ID:** `GET /api/schools/{id}`
*   **Update School:** `PUT /api/schools/{id}`
*   **Delete School:** `DELETE /api/schools/{id}`
*   **Upload School Logo:** `POST /api/schools/{id}/upload-logo`

### 3.4. User Module

*   **Get All Users:** `GET /api/users`
*   **Get User by ID:** `GET /api/users/{id}`
*   **Update User:** `PUT /api/users/{id}`
*   **Delete User:** `DELETE /api/users/{id}`

### 3.5. Subject Module

*   **Create Subject:** `POST /api/subjects`
*   **Get All Subjects:** `GET /api/subjects`
*   **Get Subject by ID:** `GET /api/subjects/{id}`
*   **Update Subject:** `PUT /api/subjects/{id}`
*   **Delete Subject:** `DELETE /api/subjects/{id}`
*   **Get Enrolled Subjects:** `GET /api/subjects/my-enrolled/{studentId}`

### 3.6. Enrollment Module

*   **Create Enrollment:** `POST /api/enrollments`
*   **Get All Enrollments:** `GET /api/enrollments`
*   **Get Enrollment by ID:** `GET /api/enrollments/{id}`
*   **Update Enrollment:** `PUT /api/enrollments/{id}`
*   **Delete Enrollment:** `DELETE /api/enrollments/{id}`

### 3.7. Lesson Module

*   **Create Lesson:** `POST /api/lesson`
*   **Get All Lessons:** `GET /api/lesson`
*   **Get Lesson by ID:** `GET /api/lesson/{id}`
*   **Get Lessons by Subject:** `GET /api/lesson/by-subject/{subjectId}`
*   **Update Lesson:** `PUT /api/lesson/{id}`
*   **Delete Lesson:** `DELETE /api/lesson/{id}`

### 3.8. Material Module

*   **Upload Material:** `POST /api/materials/upload`
*   **Download Material:** `GET /api/materials/{id}/download`

### 3.9. School Year Module

*   **Create School Year:** `POST /api/schoolyears`
*   **Get All School Years:** `GET /api/schoolyears`
*   **Get School Year by ID:** `GET /api/schoolyears/{id}`
*   **Update School Year:** `PUT /api/schoolyears/{id}`
*   **Delete School Year:** `DELETE /api/schoolyears/{id}`

### 3.10. Term Module

*   **Create Term:** `POST /api/terms`
*   **Get All Terms:** `GET /api/terms`
*   **Get Term by ID:** `GET /api/terms/{id}`
*   **Update Term:** `PUT /api/terms/{id}`
*   **Delete Term:** `DELETE /api/terms/{id}`

## 4. Data Models

This section describes the main data entities and their relationships.

### 4.1. User
Represents a user of the system (Admin, Teacher, or Student).

| Attribute | Type | Description |
|---|---|---|
| Id | int | Primary Key |
| FullName | string | |
| Username | string | Unique |
| PasswordHash | string | |
| Email | string | Unique |
| PhoneNumber | string | Nullable |
| IsActive | bool | |
| IsDeleted | bool | |
| RoleId | int | Foreign Key to Role |
| SchoolId | int | Foreign Key to School |

**Relationships:**
-   Belongs to one `Role`.
-   Belongs to one `School`.
-   Has many `Enrollments`.
-   Has many `Subjects` (as a teacher).
-   Has one `StudentDetails` (if student).

### 4.2. Role
Represents a user role.

| Attribute | Type | Description |
|---|---|---|
| Id | int | Primary Key |
| Name | string | Unique (e.g., "Admin", "Teacher", "Student") |
| IsActive | bool | |

**Relationships:**
-   Has many `Users`.

### 4.3. Organization
Represents an educational organization that can have multiple schools.

| Attribute | Type | Description |
|---|---|---|
| Id | int | Primary Key |
| Name | string | |
| Address | string | |
| PhoneNumber | string | |
| Email | string | |
| Logo | byte[] | Nullable |
| IsActive | bool | |
| IsDeleted | bool | |

**Relationships:**
-   Has many `Schools`.

### 4.4. School
Represents an individual school within an organization.

| Attribute | Type | Description |
|---|---|---|
| Id | int | Primary Key |
| Name | string | |
| Address | string | |
| PhoneNumber | string | |
| Email | string | |
| OrganizationId | int | Foreign Key to Organization |
| Logo | byte[] | Nullable |
| IsActive | bool | |
| IsDeleted | bool | |

**Relationships:**
-   Belongs to one `Organization`.
-   Has many `Users` (PeopleHere).
-   Has many `Subjects`.
-   Has many `SchoolYears`.

### 4.5. SchoolYear
Represents an academic year.

| Attribute | Type | Description |
|---|---|---|
| Id | int | Primary Key |
| Year | int | |
| StartDate | DateTime | |
| EndDate | DateTime | |
| SchoolId | int | Foreign Key to School |
| IsActive | bool | |
| IsDeleted | bool | |

**Relationships:**
-   Belongs to one `School`.
-   Has many `Terms`.

### 4.6. Term
Represents a term or semester within a school year.

| Attribute | Type | Description |
|---|---|---|
| Id | int | Primary Key |
| Name | string | (e.g., "Fall 2024", "Spring 2025") |
| StartDate | DateTime | |
| EndDate | DateTime | |
| SchoolYearId | int | Foreign Key to SchoolYear |
| IsActive | bool | |
| IsDeleted | bool | |

**Relationships:**
-   Belongs to one `SchoolYear`.
-   Has many `Enrollments`.

### 4.7. Subject
Represents a course or subject.

| Attribute | Type | Description |
|---|---|---|
| Id | int | Primary Key |
| Name | string | |
| Code | string | Unique subject code |
| Credits | int | |
| SchoolId | int | Foreign Key to School |
| TeacherId | int | Foreign Key to User (Teacher) |
| IsActive | bool | |
| IsDeleted | bool | |

**Relationships:**
-   Belongs to one `School`.
-   Belongs to one `User` (Teacher).
-   Has many `Enrollments`.
-   Has many `Lessons`.

### 4.8. Enrollment
Represents a student's enrollment in a subject for a specific term.

| Attribute | Type | Description |
|---|---|---|
| Id | int | Primary Key |
| StudentId | int | Foreign Key to User (Student) |
| SubjectId | int | Foreign Key to Subject |
| TermId | int | Foreign Key to Term |
| Grade | decimal | Nullable |
| IsActive | bool | |
| IsDeleted | bool | |

**Relationships:**
-   Belongs to one `User` (Student).
-   Belongs to one `Subject`.
-   Belongs to one `Term`.

### 4.9. Lesson
Represents a single lesson or topic within a subject.

| Attribute | Type | Description |
|---|---|---|
| Id | int | Primary Key |
| Title | string | |
| Description | string | Nullable |
| SubjectId | int | Foreign Key to Subject |
| IsDeleted | bool | |

**Relationships:**
-   Belongs to one `Subject`.
-   Has many `Materials`.

### 4.10. Material
Represents educational material (e.g., PDF, video) for a lesson.

| Attribute | Type | Description |
|---|---|---|
| Id | int | Primary Key |
| FileName | string | |
| ContentType | string | MIME type |
| FileType | string | |
| FileSize | long | |
| FileData | byte[] | |
| UploadDate | DateTime | |
| Description | string | Nullable |
| LessonId | int | Foreign Key to Lesson |
| IsDeleted | bool | |

**Relationships:**
-   Belongs to one `Lesson`.

### 4.11. StudentDetails
Stores additional details for students.

| Attribute | Type | Description |
|---|---|---|
| Id | int | Primary Key |
| StudentId | int | Foreign Key to User (Student) |
| GPA | decimal | Nullable |
| AttendanceRate | decimal | Nullable |
| ParticipationRating | decimal | Nullable |
| IsActive | bool | |

**Relationships:**
-   Belongs to one `User` (Student).

### 4.12. EmailVerification
Stores information for email verification.

| Attribute | Type | Description |
|---|---|---|
| Id | int | Primary Key |
| Email | string | |
| TempPassword | string | |
| ExpiresAt | DateTime | |
| IsVerified | bool | |

## 5. Non-Functional Requirements

*   **Security:** The application must be secure, with proper authentication and authorization mechanisms. All sensitive data should be encrypted.
*   **Performance:** The application should be performant, with fast response times for all API endpoints.
*   **Scalability:** The application should be scalable to handle a large number of users and data.
*   **Reliability:** The application should be reliable and available 24/7.
*   **Maintainability:** The code should be well-structured, documented, and easy to maintain.
