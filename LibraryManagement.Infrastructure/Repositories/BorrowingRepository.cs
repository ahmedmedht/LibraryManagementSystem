using LibraryManagement.Application.Borrowings.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BorrowingRepository : IBorrowingRepository
    {
        private readonly ApplicationDbContext _context;

        public BorrowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<BorrowingTransaction>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.BorrowingTransactions
                .AsNoTracking()
                .Include(transaction => transaction.Book)
                .Include(transaction => transaction.Member)
                .OrderByDescending(transaction => transaction.BorrowedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<BorrowingTransaction?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _context.BorrowingTransactions
                .Include(transaction => transaction.Book)
                .Include(transaction => transaction.Member)
                .FirstOrDefaultAsync(transaction => transaction.Id == id, cancellationToken);
        }

        public async Task AddAsync(
            BorrowingTransaction transaction,
            CancellationToken cancellationToken = default)
        {
            await _context.BorrowingTransactions.AddAsync(transaction, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
