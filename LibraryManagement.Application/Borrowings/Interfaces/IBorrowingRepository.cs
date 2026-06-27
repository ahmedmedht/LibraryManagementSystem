using LibraryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Borrowings.Interfaces
{
    public interface IBorrowingRepository
    {
        Task<IReadOnlyList<BorrowingTransaction>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<BorrowingTransaction?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            BorrowingTransaction transaction,
            CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
