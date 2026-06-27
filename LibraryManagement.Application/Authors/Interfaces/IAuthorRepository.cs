using LibraryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Authors.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> ExistsByNameAsync(
            string fullName,
            int? excludedAuthorId = null,
            CancellationToken cancellationToken = default);

        Task<bool> HasBooksAsync(int authorId, CancellationToken cancellationToken = default);

        Task AddAsync(Author author, CancellationToken cancellationToken = default);

        void Remove(Author author);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
