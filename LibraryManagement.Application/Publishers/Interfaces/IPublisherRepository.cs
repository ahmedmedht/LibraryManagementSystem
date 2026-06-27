using LibraryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Publishers.Interfaces
{
    public interface IPublisherRepository
    {
        Task<IReadOnlyList<Publisher>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Publisher?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> ExistsByNameAsync(
            string name,
            int? excludedPublisherId = null,
            CancellationToken cancellationToken = default);

        Task<bool> HasBooksAsync(int publisherId, CancellationToken cancellationToken = default);

        Task AddAsync(Publisher publisher, CancellationToken cancellationToken = default);

        void Remove(Publisher publisher);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
