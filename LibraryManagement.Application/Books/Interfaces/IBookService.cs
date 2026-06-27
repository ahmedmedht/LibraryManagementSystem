using LibraryManagement.Application.Books.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Books.Interfaces
{
    public interface IBookService
    {
        Task<IReadOnlyList<BookResponse>> SearchAsync(
            BookSearchRequest request,
            CancellationToken cancellationToken = default);

        Task<BookResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<BookResponse> CreateAsync(
            CreateBookRequest request,
            CancellationToken cancellationToken = default);

        Task<BookResponse> UpdateAsync(
            int id,
            UpdateBookRequest request,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
