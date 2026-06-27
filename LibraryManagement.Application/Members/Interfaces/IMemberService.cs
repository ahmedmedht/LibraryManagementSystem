using LibraryManagement.Application.Members.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Members.Interfaces
{
    public interface IMemberService
    {
        Task<IReadOnlyList<MemberResponse>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<MemberResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<MemberResponse> CreateAsync(CreateMemberRequest request, CancellationToken cancellationToken = default);

        Task<MemberResponse> UpdateAsync(int id, UpdateMemberRequest request, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task ActivateAsync(int id, CancellationToken cancellationToken = default);

        Task DeactivateAsync(int id, CancellationToken cancellationToken = default);
    }
}
