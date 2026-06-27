using LibraryManagement.Application.Authors.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Authors.Interfaces
{
    public interface IAuthorService
    {
        Task<IReadOnlyList<AuthorResponse>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<AuthorResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<AuthorResponse> CreateAsync(CreateAuthorRequest request, CancellationToken cancellationToken = default);

        Task<AuthorResponse> UpdateAsync(int id, UpdateAuthorRequest request, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
