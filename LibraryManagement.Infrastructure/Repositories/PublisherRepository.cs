using LibraryManagement.Application.Publishers.Interfaces;
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
    public class PublisherRepository : IPublisherRepository
    {
        private readonly ApplicationDbContext _context;

        public PublisherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Publisher>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Publishers
                .AsNoTracking()
                .OrderBy(publisher => publisher.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<Publisher?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Publishers
                .FirstOrDefaultAsync(publisher => publisher.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(
            string name,
            int? excludedPublisherId = null,
            CancellationToken cancellationToken = default)
        {
            var normalizedName = name.Trim().ToLower();

            return await _context.Publishers
                .AnyAsync(publisher =>
                    publisher.Name.ToLower() == normalizedName &&
                    (!excludedPublisherId.HasValue || publisher.Id != excludedPublisherId.Value),
                    cancellationToken);
        }

        public async Task<bool> HasBooksAsync(
            int publisherId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Books
                .AnyAsync(book => book.PublisherId == publisherId, cancellationToken);
        }

        public async Task AddAsync(
            Publisher publisher,
            CancellationToken cancellationToken = default)
        {
            await _context.Publishers.AddAsync(publisher, cancellationToken);
        }

        public void Remove(Publisher publisher)
        {
            _context.Publishers.Remove(publisher);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
