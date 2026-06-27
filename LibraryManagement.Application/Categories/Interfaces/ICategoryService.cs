using LibraryManagement.Application.Categories.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Categories.Interfaces
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<CategoryTreeResponse>> GetTreeAsync(CancellationToken cancellationToken = default);

        Task<CategoryResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<CategoryResponse> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default);

        Task<CategoryResponse> UpdateAsync(int id, UpdateCategoryRequest request, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
