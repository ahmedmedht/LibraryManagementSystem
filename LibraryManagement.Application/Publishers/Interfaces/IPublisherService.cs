using LibraryManagement.Application.Publishers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Publishers.Interfaces
{
    public interface IPublisherService
    {
        Task<IReadOnlyList<PublisherResponse>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<PublisherResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<PublisherResponse> CreateAsync(CreatePublisherRequest request, CancellationToken cancellationToken = default);

        Task<PublisherResponse> UpdateAsync(int id, UpdatePublisherRequest request, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
