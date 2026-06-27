using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Exceptions;

namespace LibraryManagement.Domain.Entities;
public class BorrowingTransaction
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public Book Book { get; set; } = null!;

    public int MemberId { get; set; }

    public Member Member { get; set; } = null!;

    public DateTime BorrowedAt { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ReturnedAt { get; set; }

    public BorrowingStatus Status { get; set; }

    public string BorrowedByUserId { get; set; } = string.Empty;

    public string? ReturnedByUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public static BorrowingTransaction Create(
        int bookId,
        int memberId,
        DateTime dueDate,
        string borrowedByUserId)
    {
        if (bookId <= 0)
            throw new DomainException("Book id is required.");

        if (memberId <= 0)
            throw new DomainException("Member id is required.");

        if (dueDate.Date <= DateTime.UtcNow.Date)
            throw new DomainException("Due date must be in the future.");

        if (string.IsNullOrWhiteSpace(borrowedByUserId))
            throw new DomainException("Borrowed by user id is required.");

        return new BorrowingTransaction
        {
            BookId = bookId,
            MemberId = memberId,
            BorrowedAt = DateTime.UtcNow,
            DueDate = dueDate,
            Status = BorrowingStatus.Borrowed,
            BorrowedByUserId = borrowedByUserId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkAsReturned(string returnedByUserId)
    {
        if (Status == BorrowingStatus.Returned)
            throw new DomainException("This borrowing transaction is already returned.");

        if (string.IsNullOrWhiteSpace(returnedByUserId))
            throw new DomainException("Returned by user id is required.");

        ReturnedAt = DateTime.UtcNow;
        ReturnedByUserId = returnedByUserId;
        Status = BorrowingStatus.Returned;
        UpdatedAt = DateTime.UtcNow;
    }
}