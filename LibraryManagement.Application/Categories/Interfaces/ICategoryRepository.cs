using LibraryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Categories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> ExistsByNameAsync(
            string name,
            int? excludedCategoryId = null,
            CancellationToken cancellationToken = default);

        Task<bool> HasChildrenAsync(int categoryId, CancellationToken cancellationToken = default);

        Task<bool> HasBooksAsync(int categoryId, CancellationToken cancellationToken = default);

        Task AddAsync(Category category, CancellationToken cancellationToken = default);

        void Remove(Category category);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
