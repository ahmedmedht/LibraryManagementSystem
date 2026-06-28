# Library Management System

A Library Management System built with ASP.NET Core Web API and SQL Server.

The system allows library staff to manage books, authors, publishers, categories, members, system users, and borrowing transactions.

---

## Technologies Used

- ASP.NET Core Web API
- .NET 9
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Authentication
- Role-Based Authorization
- FluentValidation
- Serilog
- Swagger / OpenAPI

---

## Architecture

The project follows a simple Clean Architecture / layered structure:

```text
LibraryManagement.Api
LibraryManagement.Application
LibraryManagement.Domain
LibraryManagement.Infrastructure
```

### LibraryManagement.Api

Contains:

- Controllers
- Middleware
- Swagger configuration
- JWT authentication setup
- Current user service
- Program.cs

### LibraryManagement.Application

Contains:

- DTOs
- Service interfaces
- Application services
- Repository interfaces
- Validators
- Security constants

### LibraryManagement.Domain

Contains:

- Entities
- Enums
- Domain exceptions
- Business rules inside entities

### LibraryManagement.Infrastructure

Contains:

- Entity Framework Core DbContext
- EF Core configurations
- Repositories
- Identity user implementation
- JWT token generation
- Authentication service
- Database seeding

---

## Main Features

- Book management
- Author management
- Publisher management
- Hierarchical category management
- Member management
- Borrowing and return management
- System user management
- JWT authentication
- Role-based authorization
- Activity logging
- Book search by name, author, category, and status
- Global exception handling
- Request logging using Serilog

---

## Authentication and Authorization

The system uses ASP.NET Core Identity for secure user management and password hashing.

Authentication is handled using JWT tokens.

Authorization is role-based.

### Supported Roles

```text
Administrator
Librarian
Staff
```

### Role Permissions

| Feature | Administrator | Librarian | Staff |
|---|---:|---:|---:|
| Login | Yes | Yes | Yes |
| Manage Books | Yes | Yes | No |
| Manage Authors | Yes | Yes | No |
| Manage Publishers | Yes | Yes | No |
| Manage Categories | Yes | Yes | No |
| Manage Members | Yes | Yes | Yes |
| Borrow / Return Books | Yes | Yes | Yes |
| Manage System Users | Yes | No | No |
| View Activity Logs | Yes | No | No |

---

## Default Seeded Users

The application seeds three default system users:

| Role | Email | Password |
|---|---|---|
| Administrator | admin@library.com | Admin@123 |
| Librarian | librarian@library.com | Librarian@123 |
| Staff | staff@library.com | Staff@123 |

---

## Database Design Summary

The system contains the following main entities:

- Books
- Authors
- BookAuthors
- Publishers
- Categories
- BookCategories
- Members
- BorrowingTransactions
- ActivityLogs
- AspNetUsers
- AspNetRoles
- AspNetUserRoles

---

## Book Copies Design

For simplicity, the system does not use a separate `BookCopy` table.

Instead, each book has:

```text
TotalCopies
AvailableCopies
Status
```

The book status is calculated using this rule:

```text
AvailableCopies > 0  => In
AvailableCopies == 0 => Out
```

Borrowing a book decreases `AvailableCopies`.

Returning a book increases `AvailableCopies`.

This design is simple, practical, and suitable for the assignment scope.

---

## Book Metadata

Each book supports:

- Title
- ISBN
- Publication year
- Edition
- Summary
- Cover image URL
- Language
- Publisher
- Multiple authors
- Multiple categories
- Total copies
- Available copies
- Status

---

## Category Hierarchy

Categories support parent-child relationships.

Example:

```text
Technology
└── Programming
```

---

## Activity Logging

The system logs important actions such as:

- Login
- Create book
- Update book
- Delete book
- Create author
- Update author
- Delete author
- Create publisher
- Update publisher
- Delete publisher
- Create category
- Update category
- Delete category
- Create member
- Update member
- Delete member
- Borrow book
- Return book
- Create user
- Update user
- Delete user
- Change user role

Activity logs are stored in the database.

Only Administrators can view activity logs.

---

## API Endpoints Summary

### Auth

```http
POST /api/auth/login
GET  /api/auth/me
```

### Authors

```http
GET    /api/authors
GET    /api/authors/{id}
POST   /api/authors
PUT    /api/authors/{id}
DELETE /api/authors/{id}
```

### Publishers

```http
GET    /api/publishers
GET    /api/publishers/{id}
POST   /api/publishers
PUT    /api/publishers/{id}
DELETE /api/publishers/{id}
```

### Categories

```http
GET    /api/categories
GET    /api/categories/tree
GET    /api/categories/{id}
POST   /api/categories
PUT    /api/categories/{id}
DELETE /api/categories/{id}
```

### Members

```http
GET    /api/members
GET    /api/members/{id}
POST   /api/members
PUT    /api/members/{id}
DELETE /api/members/{id}
```

### Books

```http
GET    /api/books
GET    /api/books/{id}
POST   /api/books
PUT    /api/books/{id}
DELETE /api/books/{id}
```

### Book Search

```http
GET /api/books?name=clean
GET /api/books?author=martin
GET /api/books?categoryId=1
GET /api/books?status=1
```

Book status values:

```text
In  = 1
Out = 2
```

### Borrowings

```http
GET  /api/borrowings
GET  /api/borrowings/{id}
POST /api/borrowings
POST /api/borrowings/{id}/return
```

### System Users

```http
GET    /api/users
GET    /api/users/{id}
POST   /api/users
PUT    /api/users/{id}
DELETE /api/users/{id}
PUT    /api/users/{id}/role
PUT    /api/users/{id}/activate
PUT    /api/users/{id}/deactivate
```

### Activity Logs

```http
GET /api/activity-logs
```

---

## Setup Instructions

### 1. Clone the repository

```bash
git clone https://github.com/ahmedmedht/LibraryManagementSystem.git
cd YOUR_REPOSITORY_NAME
```

### 2. Update the connection string

Open:

```text
LibraryManagement.Api/appsettings.json
```

Update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=LibraryManagementDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### 3. Apply database migrations

Run this command from Package Manager Console:

```powershell
Update-Database -Project LibraryManagement.Infrastructure -StartupProject LibraryManagement.Api -Context ApplicationDbContext
```

### 4. Run the API

Set `LibraryManagement.Api` as the startup project.

Run the project.

Swagger will open at:

```text
/swagger
```

---

## Authentication Flow

### Login

```http
POST /api/auth/login
```

Request body:

```json
{
  "email": "admin@library.com",
  "password": "Admin@123"
}
```

Copy the returned JWT token.

In Swagger, click `Authorize` and paste the token.

---

## Sample Test Flow

1. Login as Administrator.
2. Create an author.
3. Create a publisher.
4. Create a parent category.
5. Create a child category.
6. Create a member.
7. Create a book.
8. Search for the book by title, author, category, or status.
9. Borrow the book.
10. Return the book.
11. Check activity logs.
12. Create a new system user.
13. Test role-based access.

---

## Example Create Author Request

```json
{
  "fullName": "Robert C. Martin",
  "bio": "Software engineer and author."
}
```

---

## Example Create Publisher Request

```json
{
  "name": "Prentice Hall",
  "address": "United States",
  "website": "https://www.pearson.com"
}
```

---

## Example Create Category Request

```json
{
  "name": "Technology",
  "parentCategoryId": null
}
```

---

## Example Create Member Request

```json
{
  "fullName": "Ahmed Mohamed",
  "email": "ahmed.member@example.com",
  "phoneNumber": "01000000000",
  "address": "Cairo, Egypt",
  "membershipNumber": "MEM-001"
}
```

---

## Example Create Book Request

```json
{
  "title": "Clean Code",
  "isbn": "9780132350884",
  "publicationYear": 2008,
  "edition": "1st Edition",
  "summary": "A handbook of agile software craftsmanship.",
  "coverImageUrl": "https://example.com/clean-code.jpg",
  "language": "English",
  "publisherId": 1,
  "totalCopies": 3,
  "authorIds": [
    1
  ],
  "categoryIds": [
    2
  ]
}
```

---

## Example Borrow Book Request

```json
{
  "bookId": 1,
  "memberId": 1,
  "dueDate": "2026-12-31T00:00:00Z"
}
```

---

## Example Create System User Request

```json
{
  "fullName": "Default Staff Two",
  "email": "staff2@library.com",
  "password": "Staff2@123",
  "role": "Staff"
}
```

---

## Security Notes

- Passwords are securely hashed using ASP.NET Core Identity.
- JWT tokens are used for authentication.
- Endpoints are protected using role-based authorization.
- Users can be activated or deactivated.
- Inactive users cannot log in.
- Only Administrators can manage system users.
- Only Administrators can view activity logs.

---

## Logging

The application uses Serilog for request and application logging.

Logs are written to:

```text
Logs/
```

---

## Error Handling

The API uses a global exception handling middleware.

Validation errors return:

```json
{
  "statusCode": 400,
  "message": "Validation failed.",
  "errors": []
}
```

Business errors return:

```json
{
  "statusCode": 400,
  "message": "Error message"
}
```

Unauthorized requests return:

```json
{
  "statusCode": 401,
  "message": "Error message"
}
```

---

## Design Decisions

### Monolithic API

This project was implemented as a monolithic ASP.NET Core Web API.

Microservices were intentionally avoided because the assignment scope is focused on a single Library Management System with one relational database.

### Layered Architecture

The project uses a simple layered architecture to keep the code organized, testable, and easy to explain.

### Repository and Service Pattern

Repositories handle database access.

Services handle business logic.

Controllers only handle HTTP requests and responses.

### Identity for System Users

System users are represented using ASP.NET Core Identity.

The system does not use a separate employee table.

Internal users are managed through:

```text
AspNetUsers
AspNetRoles
AspNetUserRoles
```

### Book Copy Tracking

The system tracks copies using `TotalCopies` and `AvailableCopies`.

This keeps the design simple while still supporting borrowing and returning books.

---

## Final Notes

This project satisfies the main assignment requirements:

- Relational database design
- Book metadata management
- Multiple authors per book
- Hierarchical categories
- Publisher information
- Member management
- System user management
- Role-based access control
- Secure authentication
- Borrowing transactions
- Activity logging
- Search APIs
- Swagger documentation

