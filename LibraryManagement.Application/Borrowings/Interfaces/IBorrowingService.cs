using LibraryManagement.Application.Borrowings.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Borrowings.Interfaces
{
    public interface IBorrowingService
    {
        Task<IReadOnlyList<BorrowingResponse>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<BorrowingResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<BorrowingResponse> BorrowBookAsync(
            BorrowBookRequest request,
            CancellationToken cancellationToken = default);

        Task<BorrowingResponse> ReturnBookAsync(
            int borrowingId,
            CancellationToken cancellationToken = default);
    }
}
