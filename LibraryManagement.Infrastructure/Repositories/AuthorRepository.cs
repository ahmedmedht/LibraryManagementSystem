using LibraryManagement.Application.Authors.Interfaces;
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
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Author>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .AsNoTracking()
                .OrderBy(author => author.FullName)
                .ToListAsync(cancellationToken);
        }

        public async Task<Author?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .FirstOrDefaultAsync(author => author.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(
            string fullName,
            int? excludedAuthorId = null,
            CancellationToken cancellationToken = default)
        {
            var normalizedName = fullName.Trim().ToLower();

            return await _context.Authors
                .AnyAsync(author =>
                    author.FullName.ToLower() == normalizedName &&
                    (!excludedAuthorId.HasValue || author.Id != excludedAuthorId.Value),
                    cancellationToken);
        }

        public async Task<bool> HasBooksAsync(
            int authorId,
            CancellationToken cancellationToken = default)
        {
            return await _context.BookAuthors
                .AnyAsync(bookAuthor => bookAuthor.AuthorId == authorId, cancellationToken);
        }

        public async Task AddAsync(
            Author author,
            CancellationToken cancellationToken = default)
        {
            await _context.Authors.AddAsync(author, cancellationToken);
        }

        public void Remove(Author author)
        {
            _context.Authors.Remove(author);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
