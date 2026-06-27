using LibraryManagement.Application.Books.DTOs;
using LibraryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Books.Interfaces
{
    public interface IBookRepository
    {
        Task<IReadOnlyList<Book>> SearchAsync(
            BookSearchRequest request,
            CancellationToken cancellationToken = default);

        Task<Book?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<Book?> GetByIdWithDetailsAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<bool> IsbnExistsAsync(
            string isbn,
            int? excludedBookId = null,
            CancellationToken cancellationToken = default);

        Task<bool> PublisherExistsAsync(
            int publisherId,
            CancellationToken cancellationToken = default);

        Task<bool> AuthorsExistAsync(
            List<int> authorIds,
            CancellationToken cancellationToken = default);

        Task<bool> CategoriesExistAsync(
            List<int> categoryIds,
            CancellationToken cancellationToken = default);

        Task<bool> HasActiveBorrowingsAsync(
            int bookId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Book book,
            CancellationToken cancellationToken = default);

        void Remove(Book book);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
