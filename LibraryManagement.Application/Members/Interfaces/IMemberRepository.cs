using LibraryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Members.Interfaces
{
    public interface IMemberRepository
    {
        Task<IReadOnlyList<Member>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Member?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> ExistsByEmailAsync(
            string email,
            int? excludedMemberId = null,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsByMembershipNumberAsync(
            string membershipNumber,
            int? excludedMemberId = null,
            CancellationToken cancellationToken = default);

        Task<bool> HasActiveBorrowingsAsync(int memberId, CancellationToken cancellationToken = default);

        Task AddAsync(Member member, CancellationToken cancellationToken = default);

        void Remove(Member member);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
