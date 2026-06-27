using FluentValidation;
using LibraryManagement.Application.ActivityLogs.Interfaces;
using LibraryManagement.Application.Books.Interfaces;
using LibraryManagement.Application.Borrowings.DTOs;
using LibraryManagement.Application.Borrowings.Interfaces;
using LibraryManagement.Application.Common.Interfaces;
using LibraryManagement.Application.Members.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;


namespace LibraryManagement.Application.Borrowings.Services
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IActivityLogger _activityLogger;
        private readonly IValidator<BorrowBookRequest> _borrowValidator;

        public BorrowingService(
            IBorrowingRepository borrowingRepository,
            IBookRepository bookRepository,
            IMemberRepository memberRepository,
            ICurrentUserService currentUserService,
            IActivityLogger activityLogger,
            IValidator<BorrowBookRequest> borrowValidator)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
            _currentUserService = currentUserService;
            _activityLogger = activityLogger;
            _borrowValidator = borrowValidator;
        }

        public async Task<IReadOnlyList<BorrowingResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var transactions = await _borrowingRepository.GetAllAsync(cancellationToken);

            return transactions
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<BorrowingResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var transaction = await _borrowingRepository.GetByIdAsync(id, cancellationToken);

            return transaction is null ? null : MapToResponse(transaction);
        }

        public async Task<BorrowingResponse> BorrowBookAsync(
            BorrowBookRequest request,
            CancellationToken cancellationToken = default)
        {
            await _borrowValidator.ValidateAndThrowAsync(request, cancellationToken);

            var userId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            var member = await _memberRepository.GetByIdAsync(
                request.MemberId,
                cancellationToken);

            if (member is null)
                throw new InvalidOperationException("Member was not found.");

            if (!member.IsActive)
                throw new InvalidOperationException("Inactive members cannot borrow books.");

            var book = await _bookRepository.GetByIdAsync(
                request.BookId,
                cancellationToken);

            if (book is null)
                throw new InvalidOperationException("Book was not found.");

            book.BorrowOneCopy();

            var transaction = BorrowingTransaction.Create(
                book.Id,
                member.Id,
                request.DueDate,
                userId);

            await _borrowingRepository.AddAsync(transaction, cancellationToken);
            await _borrowingRepository.SaveChangesAsync(cancellationToken);

            var createdTransaction = await _borrowingRepository.GetByIdAsync(
                transaction.Id,
                cancellationToken);

            await _activityLogger.LogAsync(
                ActivityAction.BorrowBook,
                nameof(BorrowingTransaction),
                transaction.Id.ToString(),
                $"Book '{book.Title}' was borrowed by member '{member.FullName}'.",
                cancellationToken);

            return MapToResponse(createdTransaction!);
        }

        public async Task<BorrowingResponse> ReturnBookAsync(
            int borrowingId,
            CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            var transaction = await _borrowingRepository.GetByIdAsync(
                borrowingId,
                cancellationToken);

            if (transaction is null)
                throw new InvalidOperationException("Borrowing transaction was not found.");

            transaction.MarkAsReturned(userId);

            transaction.Book.ReturnOneCopy();

            await _borrowingRepository.SaveChangesAsync(cancellationToken);

            await _activityLogger.LogAsync(
                ActivityAction.ReturnBook,
                nameof(BorrowingTransaction),
                transaction.Id.ToString(),
                $"Book '{transaction.Book.Title}' was returned by member '{transaction.Member.FullName}'.",
                cancellationToken);

            return MapToResponse(transaction);
        }

        private static BorrowingResponse MapToResponse(BorrowingTransaction transaction)
        {
            return new BorrowingResponse
            {
                Id = transaction.Id,
                BookId = transaction.BookId,
                BookTitle = transaction.Book?.Title ?? string.Empty,
                MemberId = transaction.MemberId,
                MemberName = transaction.Member?.FullName ?? string.Empty,
                BorrowedAt = transaction.BorrowedAt,
                DueDate = transaction.DueDate,
                ReturnedAt = transaction.ReturnedAt,
                Status = transaction.Status,
                BorrowedByUserId = transaction.BorrowedByUserId,
                ReturnedByUserId = transaction.ReturnedByUserId
            };
        }
    }
}
