using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Exceptions;

namespace LibraryManagement.Domain.Entities;

public class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ISBN { get; set; } = string.Empty;

    public int PublicationYear { get; set; }

    public string? Edition { get; set; }

    public string? Summary { get; set; }

    public string? CoverImageUrl { get; set; }

    public string Language { get; set; } = string.Empty;

    public int PublisherId { get; set; }

    public Publisher Publisher { get; set; } = null!;

    public int TotalCopies { get; set; }

    public int AvailableCopies { get; set; }

    public BookStatus Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();

    public ICollection<BorrowingTransaction> BorrowingTransactions { get; set; } = new List<BorrowingTransaction>();

    public void SetInitialCopies(int totalCopies)
    {
        if (totalCopies <= 0)
            throw new DomainException("Total copies must be greater than zero.");

        TotalCopies = totalCopies;
        AvailableCopies = totalCopies;
        RefreshStatus();
    }

    public void UpdateCopies(int totalCopies, int availableCopies)
    {
        if (totalCopies <= 0)
            throw new DomainException("Total copies must be greater than zero.");

        if (availableCopies < 0)
            throw new DomainException("Available copies cannot be less than zero.");

        if (availableCopies > totalCopies)
            throw new DomainException("Available copies cannot exceed total copies.");

        TotalCopies = totalCopies;
        AvailableCopies = availableCopies;
        UpdatedAt = DateTime.UtcNow;

        RefreshStatus();
    }

    public void BorrowOneCopy()
    {
        if (AvailableCopies <= 0)
            throw new DomainException("This book is currently out of stock.");

        AvailableCopies -= 1;
        UpdatedAt = DateTime.UtcNow;

        RefreshStatus();
    }

    public void ReturnOneCopy()
    {
        if (AvailableCopies >= TotalCopies)
            throw new DomainException("Available copies cannot exceed total copies.");

        AvailableCopies += 1;
        UpdatedAt = DateTime.UtcNow;

        RefreshStatus();
    }

    public void RefreshStatus()
    {
        Status = AvailableCopies > 0 ? BookStatus.In : BookStatus.Out;
    }
}