USE LibraryManagementDb;
GO

DECLARE @Now DATETIME2 = SYSUTCDATETIME();

------------------------------------------------------------
-- Publishers
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM Publishers WHERE Name = 'Prentice Hall')
BEGIN
    INSERT INTO Publishers (Name, Address, Website, CreatedAt)
    VALUES ('Prentice Hall', 'United States', 'https://www.pearson.com', @Now);
END

IF NOT EXISTS (SELECT 1 FROM Publishers WHERE Name = 'Addison-Wesley')
BEGIN
    INSERT INTO Publishers (Name, Address, Website, CreatedAt)
    VALUES ('Addison-Wesley', 'United States', 'https://www.pearson.com', @Now);
END

------------------------------------------------------------
-- Authors
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM Authors WHERE FullName = 'Robert C. Martin')
BEGIN
    INSERT INTO Authors (FullName, Bio, CreatedAt)
    VALUES ('Robert C. Martin', 'Software engineer and author.', @Now);
END

IF NOT EXISTS (SELECT 1 FROM Authors WHERE FullName = 'Martin Fowler')
BEGIN
    INSERT INTO Authors (FullName, Bio, CreatedAt)
    VALUES ('Martin Fowler', 'Software engineer, author, and speaker.', @Now);
END

------------------------------------------------------------
-- Categories
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = 'Technology')
BEGIN
    INSERT INTO Categories (Name, ParentCategoryId, CreatedAt)
    VALUES ('Technology', NULL, @Now);
END

DECLARE @TechnologyCategoryId INT =
(
    SELECT TOP 1 Id
    FROM Categories
    WHERE Name = 'Technology'
);

IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = 'Programming')
BEGIN
    INSERT INTO Categories (Name, ParentCategoryId, CreatedAt)
    VALUES ('Programming', @TechnologyCategoryId, @Now);
END

IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = 'Software Architecture')
BEGIN
    INSERT INTO Categories (Name, ParentCategoryId, CreatedAt)
    VALUES ('Software Architecture', @TechnologyCategoryId, @Now);
END

------------------------------------------------------------
-- Members
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM Members WHERE MembershipNumber = 'MEM-001')
BEGIN
    INSERT INTO Members
    (
        FullName,
        Email,
        PhoneNumber,
        Address,
        MembershipNumber,
        IsActive,
        CreatedAt
    )
    VALUES
    (
        'Ahmed Mohamed',
        'ahmed.member@example.com',
        '01000000000',
        'Cairo, Egypt',
        'MEM-001',
        1,
        @Now
    );
END

IF NOT EXISTS (SELECT 1 FROM Members WHERE MembershipNumber = 'MEM-002')
BEGIN
    INSERT INTO Members
    (
        FullName,
        Email,
        PhoneNumber,
        Address,
        MembershipNumber,
        IsActive,
        CreatedAt
    )
    VALUES
    (
        'Sara Ali',
        'sara.member@example.com',
        '01111111111',
        'Alexandria, Egypt',
        'MEM-002',
        1,
        @Now
    );
END

------------------------------------------------------------
-- Books
------------------------------------------------------------
DECLARE @PrenticeHallId INT =
(
    SELECT TOP 1 Id
    FROM Publishers
    WHERE Name = 'Prentice Hall'
);

DECLARE @AddisonWesleyId INT =
(
    SELECT TOP 1 Id
    FROM Publishers
    WHERE Name = 'Addison-Wesley'
);

DECLARE @ProgrammingCategoryId INT =
(
    SELECT TOP 1 Id
    FROM Categories
    WHERE Name = 'Programming'
);

DECLARE @ArchitectureCategoryId INT =
(
    SELECT TOP 1 Id
    FROM Categories
    WHERE Name = 'Software Architecture'
);

IF NOT EXISTS (SELECT 1 FROM Books WHERE ISBN = '9780132350884')
BEGIN
    INSERT INTO Books
    (
        Title,
        ISBN,
        PublicationYear,
        Edition,
        Summary,
        CoverImageUrl,
        Language,
        PublisherId,
        TotalCopies,
        AvailableCopies,
        Status,
        CreatedAt
    )
    VALUES
    (
        'Clean Code',
        '9780132350884',
        2008,
        '1st Edition',
        'A handbook of agile software craftsmanship.',
        'https://example.com/clean-code.jpg',
        'English',
        @PrenticeHallId,
        3,
        3,
        1,
        @Now
    );
END

IF NOT EXISTS (SELECT 1 FROM Books WHERE ISBN = '9780201485677')
BEGIN
    INSERT INTO Books
    (
        Title,
        ISBN,
        PublicationYear,
        Edition,
        Summary,
        CoverImageUrl,
        Language,
        PublisherId,
        TotalCopies,
        AvailableCopies,
        Status,
        CreatedAt
    )
    VALUES
    (
        'Refactoring',
        '9780201485677',
        1999,
        '1st Edition',
        'Improving the design of existing code.',
        'https://example.com/refactoring.jpg',
        'English',
        @AddisonWesleyId,
        2,
        2,
        1,
        @Now
    );
END

------------------------------------------------------------
-- Book Authors
------------------------------------------------------------
DECLARE @CleanCodeBookId INT =
(
    SELECT TOP 1 Id
    FROM Books
    WHERE ISBN = '9780132350884'
);

DECLARE @RefactoringBookId INT =
(
    SELECT TOP 1 Id
    FROM Books
    WHERE ISBN = '9780201485677'
);

DECLARE @RobertMartinId INT =
(
    SELECT TOP 1 Id
    FROM Authors
    WHERE FullName = 'Robert C. Martin'
);

DECLARE @MartinFowlerId INT =
(
    SELECT TOP 1 Id
    FROM Authors
    WHERE FullName = 'Martin Fowler'
);

IF NOT EXISTS
(
    SELECT 1
    FROM BookAuthors
    WHERE BookId = @CleanCodeBookId
      AND AuthorId = @RobertMartinId
)
BEGIN
    INSERT INTO BookAuthors (BookId, AuthorId)
    VALUES (@CleanCodeBookId, @RobertMartinId);
END

IF NOT EXISTS
(
    SELECT 1
    FROM BookAuthors
    WHERE BookId = @RefactoringBookId
      AND AuthorId = @MartinFowlerId
)
BEGIN
    INSERT INTO BookAuthors (BookId, AuthorId)
    VALUES (@RefactoringBookId, @MartinFowlerId);
END

------------------------------------------------------------
-- Book Categories
------------------------------------------------------------
IF NOT EXISTS
(
    SELECT 1
    FROM BookCategories
    WHERE BookId = @CleanCodeBookId
      AND CategoryId = @ProgrammingCategoryId
)
BEGIN
    INSERT INTO BookCategories (BookId, CategoryId)
    VALUES (@CleanCodeBookId, @ProgrammingCategoryId);
END

IF NOT EXISTS
(
    SELECT 1
    FROM BookCategories
    WHERE BookId = @RefactoringBookId
      AND CategoryId = @ArchitectureCategoryId
)
BEGIN
    INSERT INTO BookCategories (BookId, CategoryId)
    VALUES (@RefactoringBookId, @ArchitectureCategoryId);
END

PRINT 'Sample data inserted successfully.';