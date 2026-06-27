# Database Scripts

This folder contains SQL scripts for the Library Management System.

## Files

### 01-database-schema.sql

Generated from Entity Framework Core migrations.

It creates the full database schema including:

- Application tables
- ASP.NET Core Identity tables
- Foreign keys
- Indexes
- Migration history table

### 02-sample-data.sql

Adds sample data for:

- Publishers
- Authors
- Categories
- Members
- Books
- BookAuthors
- BookCategories

## How to Use

1. Run `01-database-schema.sql`.
2. Run the API once to seed Identity roles and default users.
3. Run `02-sample-data.sql`.

Default seeded users are created by the application startup code.

## Notes

The Identity users and roles are not inserted by the SQL sample data script.

They are seeded automatically when the API starts.

Default users:

| Role | Email | Password |
|---|---|---|
| Administrator | admin@library.com | Admin@123 |
| Librarian | librarian@library.com | Librarian@123 |
| Staff | staff@library.com | Staff@123 |