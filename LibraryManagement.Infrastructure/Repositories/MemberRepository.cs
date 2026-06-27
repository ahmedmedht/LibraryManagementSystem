using LibraryManagement.Application.Members.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext _context;

        public MemberRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Member>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Members
                .AsNoTracking()
                .OrderBy(member => member.FullName)
                .ToListAsync(cancellationToken);
        }

        public async Task<Member?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Members
                .FirstOrDefaultAsync(member => member.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(
            string email,
            int? excludedMemberId = null,
            CancellationToken cancellationToken = default)
        {
            var normalizedEmail = email.Trim().ToLower();

            return await _context.Members
                .AnyAsync(member =>
                    member.Email.ToLower() == normalizedEmail &&
                    (!excludedMemberId.HasValue || member.Id != excludedMemberId.Value),
                    cancellationToken);
        }

        public async Task<bool> ExistsByMembershipNumberAsync(
            string membershipNumber,
            int? excludedMemberId = null,
            CancellationToken cancellationToken = default)
        {
            var normalizedMembershipNumber = membershipNumber.Trim().ToLower();

            return await _context.Members
                .AnyAsync(member =>
                    member.MembershipNumber.ToLower() == normalizedMembershipNumber &&
                    (!excludedMemberId.HasValue || member.Id != excludedMemberId.Value),
                    cancellationToken);
        }

        public async Task<bool> HasActiveBorrowingsAsync(
            int memberId,
            CancellationToken cancellationToken = default)
        {
            return await _context.BorrowingTransactions
                .AnyAsync(transaction =>
                    transaction.MemberId == memberId &&
                    transaction.Status == BorrowingStatus.Borrowed,
                    cancellationToken);
        }

        public async Task AddAsync(
            Member member,
            CancellationToken cancellationToken = default)
        {
            await _context.Members.AddAsync(member, cancellationToken);
        }

        public void Remove(Member member)
        {
            _context.Members.Remove(member);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
