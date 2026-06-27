# Entity Relationship Diagram

This file describes the database relationships for the Library Management System.

The ERD is written using Mermaid syntax and can be rendered directly on GitHub.

---

## ERD

```mermaid
erDiagram

    Publishers ||--o{ Books : publishes

    Books ||--o{ BookAuthors : has
    Authors ||--o{ BookAuthors : writes

    Books ||--o{ BookCategories : belongs_to
    Categories ||--o{ BookCategories : contains

    Categories ||--o{ Categories : parent_child

    Books ||--o{ BorrowingTransactions : borrowed_in
    Members ||--o{ BorrowingTransactions : borrows

    AspNetUsers ||--o{ BorrowingTransactions : borrowed_by
    AspNetUsers ||--o{ ActivityLogs : creates

    AspNetUsers ||--o{ AspNetUserRoles : has
    AspNetRoles ||--o{ AspNetUserRoles : assigned_to

    Publishers {
        int Id PK
        string Name
        string Address
        string Website
        datetime CreatedAt
        datetime UpdatedAt
    }

    Authors {
        int Id PK
        string FullName
        string Bio
        datetime CreatedAt
        datetime UpdatedAt
    }

    Categories {
        int Id PK
        string Name
        int ParentCategoryId FK
        datetime CreatedAt
        datetime UpdatedAt
    }

    Books {
        int Id PK
        string Title
        string ISBN
        int PublicationYear
        string Edition
        string Summary
        string CoverImageUrl
        string Language
        int PublisherId FK
        int TotalCopies
        int AvailableCopies
        int Status
        datetime CreatedAt
        datetime UpdatedAt
    }

    BookAuthors {
        int BookId PK, FK
        int AuthorId PK, FK
    }

    BookCategories {
        int BookId PK, FK
        int CategoryId PK, FK
    }

    Members {
        int Id PK
        string FullName
        string Email
        string PhoneNumber
        string Address
        string MembershipNumber
        bool IsActive
        datetime CreatedAt
        datetime UpdatedAt
    }

    BorrowingTransactions {
        int Id PK
        int BookId FK
        int MemberId FK
        datetime BorrowedAt
        datetime DueDate
        datetime ReturnedAt
        int Status
        string BorrowedByUserId FK
        string ReturnedByUserId FK
    }

    ActivityLogs {
        int Id PK
        string UserId FK
        int Action
        string EntityName
        string EntityId
        string Description
        datetime CreatedAt
    }

    AspNetUsers {
        string Id PK
        string UserName
        string Email
        string FullName
        bool IsActive
        datetime CreatedAt
        datetime UpdatedAt
    }

    AspNetRoles {
        string Id PK
        string Name
        string NormalizedName
    }

    AspNetUserRoles {
        string UserId PK, FK
        string RoleId PK, FK
    }
```

---

## Relationship Summary

### Books and Publishers

Each book has one publisher.

Each publisher can publish many books.

```text
Publisher 1 ---- * Books
```

---

### Books and Authors

A book can have multiple authors.

An author can write multiple books.

This is handled using the `BookAuthors` join table.

```text
Books * ---- * Authors
```

---

### Books and Categories

A book can belong to multiple categories.

A category can contain multiple books.

This is handled using the `BookCategories` join table.

```text
Books * ---- * Categories
```

---

### Category Hierarchy

Categories support parent-child relationships.

Example:

```text
Technology
└── Programming
```

This is handled by:

```text
Categories.ParentCategoryId
```

---

### Members and Borrowing Transactions

A member can borrow many books.

Each borrowing transaction belongs to one member and one book.

```text
Member 1 ---- * BorrowingTransactions
Book   1 ---- * BorrowingTransactions
```

---

### System Users and Borrowing Transactions

System users perform borrowing and returning actions.

The transaction stores:

```text
BorrowedByUserId
ReturnedByUserId
```

These values reference Identity users.

---

### System Users and Roles

System users are handled by ASP.NET Core Identity.

Roles are handled using:

```text
AspNetUsers
AspNetRoles
AspNetUserRoles
```

---

### Activity Logs

Activity logs track important system actions.

Each log stores:

```text
UserId
Action
EntityName
EntityId
Description
CreatedAt
```

Only administrators can view activity logs.