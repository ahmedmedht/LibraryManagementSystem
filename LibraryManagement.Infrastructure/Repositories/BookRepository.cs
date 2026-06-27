using LibraryManagement.Application.Books.DTOs;
using LibraryManagement.Application.Books.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Book>> SearchAsync(
        BookSearchRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Books
            .AsNoTracking()
            .Include(book => book.Publisher)
            .Include(book => book.BookAuthors)
                .ThenInclude(bookAuthor => bookAuthor.Author)
            .Include(book => book.BookCategories)
                .ThenInclude(bookCategory => bookCategory.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var name = request.Name.Trim().ToLower();

            query = query.Where(book =>
                book.Title.ToLower().Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(request.Author))
        {
            var author = request.Author.Trim().ToLower();

            query = query.Where(book =>
                book.BookAuthors.Any(bookAuthor =>
                    bookAuthor.Author.FullName.ToLower().Contains(author)));
        }

        if (request.CategoryId.HasValue)
        {
            query = query.Where(book =>
                book.BookCategories.Any(bookCategory =>
                    bookCategory.CategoryId == request.CategoryId.Value));
        }

        if (request.Status.HasValue)
        {
            query = query.Where(book => book.Status == request.Status.Value);
        }

        return await query
            .OrderBy(book => book.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<Book?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .FirstOrDefaultAsync(book => book.Id == id, cancellationToken);
    }

    public async Task<Book?> GetByIdWithDetailsAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(book => book.Publisher)
            .Include(book => book.BookAuthors)
                .ThenInclude(bookAuthor => bookAuthor.Author)
            .Include(book => book.BookCategories)
                .ThenInclude(bookCategory => bookCategory.Category)
            .FirstOrDefaultAsync(book => book.Id == id, cancellationToken);
    }

    public async Task<bool> IsbnExistsAsync(
        string isbn,
        int? excludedBookId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedIsbn = isbn.Trim().ToLower();

        return await _context.Books
            .AnyAsync(book =>
                book.ISBN.ToLower() == normalizedIsbn &&
                (!excludedBookId.HasValue || book.Id != excludedBookId.Value),
                cancellationToken);
    }

    public async Task<bool> PublisherExistsAsync(
        int publisherId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Publishers
            .AnyAsync(publisher => publisher.Id == publisherId, cancellationToken);
    }

    public async Task<bool> AuthorsExistAsync(
        List<int> authorIds,
        CancellationToken cancellationToken = default)
    {
        var distinctIds = authorIds.Distinct().ToList();

        var existingCount = await _context.Authors
            .CountAsync(author => distinctIds.Contains(author.Id), cancellationToken);

        return existingCount == distinctIds.Count;
    }

    public async Task<bool> CategoriesExistAsync(
        List<int> categoryIds,
        CancellationToken cancellationToken = default)
    {
        var distinctIds = categoryIds.Distinct().ToList();

        var existingCount = await _context.Categories
            .CountAsync(category => distinctIds.Contains(category.Id), cancellationToken);

        return existingCount == distinctIds.Count;
    }

    public async Task<bool> HasActiveBorrowingsAsync(
        int bookId,
        CancellationToken cancellationToken = default)
    {
        return await _context.BorrowingTransactions
            .AnyAsync(transaction =>
                transaction.BookId == bookId &&
                transaction.Status == BorrowingStatus.Borrowed,
                cancellationToken);
    }

    public async Task AddAsync(
        Book book,
        CancellationToken cancellationToken = default)
    {
        await _context.Books.AddAsync(book, cancellationToken);
    }

    public void Remove(Book book)
    {
        _context.Books.Remove(book);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}