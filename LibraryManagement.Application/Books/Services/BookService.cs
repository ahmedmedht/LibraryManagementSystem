using FluentValidation;
using LibraryManagement.Application.ActivityLogs.Interfaces;
using LibraryManagement.Application.Books.DTOs;
using LibraryManagement.Application.Books.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.Books.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IActivityLogger _activityLogger;
    private readonly IValidator<CreateBookRequest> _createValidator;
    private readonly IValidator<UpdateBookRequest> _updateValidator;

    public BookService(
        IBookRepository bookRepository,
        IActivityLogger activityLogger,
        IValidator<CreateBookRequest> createValidator,
        IValidator<UpdateBookRequest> updateValidator)
    {
        _bookRepository = bookRepository;
        _activityLogger = activityLogger;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IReadOnlyList<BookResponse>> SearchAsync(
        BookSearchRequest request,
        CancellationToken cancellationToken = default)
    {
        var books = await _bookRepository.SearchAsync(request, cancellationToken);

        return books
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<BookResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdWithDetailsAsync(id, cancellationToken);

        return book is null ? null : MapToResponse(book);
    }

    public async Task<BookResponse> CreateAsync(
        CreateBookRequest request,
        CancellationToken cancellationToken = default)
    {
        await _createValidator.ValidateAndThrowAsync(request, cancellationToken);

        var authorIds = request.AuthorIds.Distinct().ToList();
        var categoryIds = request.CategoryIds.Distinct().ToList();

        if (await _bookRepository.IsbnExistsAsync(request.ISBN, null, cancellationToken))
            throw new InvalidOperationException("A book with the same ISBN already exists.");

        if (!await _bookRepository.PublisherExistsAsync(request.PublisherId, cancellationToken))
            throw new InvalidOperationException("Publisher was not found.");

        if (!await _bookRepository.AuthorsExistAsync(authorIds, cancellationToken))
            throw new InvalidOperationException("One or more authors were not found.");

        if (!await _bookRepository.CategoriesExistAsync(categoryIds, cancellationToken))
            throw new InvalidOperationException("One or more categories were not found.");

        var book = new Book
        {
            Title = request.Title.Trim(),
            ISBN = request.ISBN.Trim(),
            PublicationYear = request.PublicationYear,
            Edition = request.Edition?.Trim(),
            Summary = request.Summary?.Trim(),
            CoverImageUrl = request.CoverImageUrl?.Trim(),
            Language = request.Language.Trim(),
            PublisherId = request.PublisherId,
            CreatedAt = DateTime.UtcNow
        };

        book.SetInitialCopies(request.TotalCopies);

        foreach (var authorId in authorIds)
        {
            book.BookAuthors.Add(new BookAuthor
            {
                AuthorId = authorId
            });
        }

        foreach (var categoryId in categoryIds)
        {
            book.BookCategories.Add(new BookCategory
            {
                CategoryId = categoryId
            });
        }

        await _bookRepository.AddAsync(book, cancellationToken);
        await _bookRepository.SaveChangesAsync(cancellationToken);

        var createdBook = await _bookRepository.GetByIdWithDetailsAsync(book.Id, cancellationToken);

        await _activityLogger.LogAsync(
            ActivityAction.CreateBook,
            nameof(Book),
            book.Id.ToString(),
            $"Book '{book.Title}' was created.",
            cancellationToken);

        return MapToResponse(createdBook!);
    }

    public async Task<BookResponse> UpdateAsync(
        int id,
        UpdateBookRequest request,
        CancellationToken cancellationToken = default)
    {
        await _updateValidator.ValidateAndThrowAsync(request, cancellationToken);

        var book = await _bookRepository.GetByIdWithDetailsAsync(id, cancellationToken);

        if (book is null)
            throw new InvalidOperationException("Book was not found.");

        var authorIds = request.AuthorIds.Distinct().ToList();
        var categoryIds = request.CategoryIds.Distinct().ToList();

        if (await _bookRepository.IsbnExistsAsync(request.ISBN, id, cancellationToken))
            throw new InvalidOperationException("Another book with the same ISBN already exists.");

        if (!await _bookRepository.PublisherExistsAsync(request.PublisherId, cancellationToken))
            throw new InvalidOperationException("Publisher was not found.");

        if (!await _bookRepository.AuthorsExistAsync(authorIds, cancellationToken))
            throw new InvalidOperationException("One or more authors were not found.");

        if (!await _bookRepository.CategoriesExistAsync(categoryIds, cancellationToken))
            throw new InvalidOperationException("One or more categories were not found.");

        book.Title = request.Title.Trim();
        book.ISBN = request.ISBN.Trim();
        book.PublicationYear = request.PublicationYear;
        book.Edition = request.Edition?.Trim();
        book.Summary = request.Summary?.Trim();
        book.CoverImageUrl = request.CoverImageUrl?.Trim();
        book.Language = request.Language.Trim();
        book.PublisherId = request.PublisherId;

        book.UpdateCopies(request.TotalCopies, request.AvailableCopies);

        UpdateBookAuthors(book, authorIds);
        UpdateBookCategories(book, categoryIds);

        await _bookRepository.SaveChangesAsync(cancellationToken);

        var updatedBook = await _bookRepository.GetByIdWithDetailsAsync(book.Id, cancellationToken);

        await _activityLogger.LogAsync(
            ActivityAction.UpdateBook,
            nameof(Book),
            book.Id.ToString(),
            $"Book '{book.Title}' was updated.",
            cancellationToken);

        return MapToResponse(updatedBook!);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdWithDetailsAsync(id, cancellationToken);

        if (book is null)
            throw new InvalidOperationException("Book was not found.");

        if (await _bookRepository.HasActiveBorrowingsAsync(id, cancellationToken))
            throw new InvalidOperationException("Cannot delete this book because it has active borrowings.");

        _bookRepository.Remove(book);
        await _bookRepository.SaveChangesAsync(cancellationToken);

        await _activityLogger.LogAsync(
            ActivityAction.DeleteBook,
            nameof(Book),
            id.ToString(),
            $"Book '{book.Title}' was deleted.",
            cancellationToken);
    }

    private static void UpdateBookAuthors(Book book, List<int> newAuthorIds)
    {
        var currentAuthorIds = book.BookAuthors
            .Select(bookAuthor => bookAuthor.AuthorId)
            .ToList();

        var authorsToRemove = book.BookAuthors
            .Where(bookAuthor => !newAuthorIds.Contains(bookAuthor.AuthorId))
            .ToList();

        foreach (var authorToRemove in authorsToRemove)
        {
            book.BookAuthors.Remove(authorToRemove);
        }

        var authorsToAdd = newAuthorIds
            .Where(authorId => !currentAuthorIds.Contains(authorId))
            .ToList();

        foreach (var authorId in authorsToAdd)
        {
            book.BookAuthors.Add(new BookAuthor
            {
                BookId = book.Id,
                AuthorId = authorId
            });
        }
    }

    private static void UpdateBookCategories(Book book, List<int> newCategoryIds)
    {
        var currentCategoryIds = book.BookCategories
            .Select(bookCategory => bookCategory.CategoryId)
            .ToList();

        var categoriesToRemove = book.BookCategories
            .Where(bookCategory => !newCategoryIds.Contains(bookCategory.CategoryId))
            .ToList();

        foreach (var categoryToRemove in categoriesToRemove)
        {
            book.BookCategories.Remove(categoryToRemove);
        }

        var categoriesToAdd = newCategoryIds
            .Where(categoryId => !currentCategoryIds.Contains(categoryId))
            .ToList();

        foreach (var categoryId in categoriesToAdd)
        {
            book.BookCategories.Add(new BookCategory
            {
                BookId = book.Id,
                CategoryId = categoryId
            });
        }
    }

    private static BookResponse MapToResponse(Book book)
    {
        return new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            ISBN = book.ISBN,
            PublicationYear = book.PublicationYear,
            Edition = book.Edition,
            Summary = book.Summary,
            CoverImageUrl = book.CoverImageUrl,
            Language = book.Language,
            PublisherId = book.PublisherId,
            PublisherName = book.Publisher?.Name ?? string.Empty,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies,
            Status = book.Status,
            Authors = book.BookAuthors
                .Select(bookAuthor => bookAuthor.Author.FullName)
                .ToList(),
            Categories = book.BookCategories
                .Select(bookCategory => bookCategory.Category.Name)
                .ToList(),
            CreatedAt = book.CreatedAt,
            UpdatedAt = book.UpdatedAt
        };
    }
}