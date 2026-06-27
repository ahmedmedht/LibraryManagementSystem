using FluentValidation;
using LibraryManagement.Application.ActivityLogs.Interfaces;
using LibraryManagement.Application.Members.DTOs;
using LibraryManagement.Application.Members.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Members.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IActivityLogger _activityLogger;
        private readonly IValidator<CreateMemberRequest> _createValidator;
        private readonly IValidator<UpdateMemberRequest> _updateValidator;

        public MemberService(
            IMemberRepository memberRepository,
            IActivityLogger activityLogger,
            IValidator<CreateMemberRequest> createValidator,
            IValidator<UpdateMemberRequest> updateValidator)
        {
            _memberRepository = memberRepository;
            _activityLogger = activityLogger;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IReadOnlyList<MemberResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var members = await _memberRepository.GetAllAsync(cancellationToken);

            return members
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<MemberResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var member = await _memberRepository.GetByIdAsync(id, cancellationToken);

            return member is null ? null : MapToResponse(member);
        }

        public async Task<MemberResponse> CreateAsync(
            CreateMemberRequest request,
            CancellationToken cancellationToken = default)
        {
            await _createValidator.ValidateAndThrowAsync(request, cancellationToken);

            var emailExists = await _memberRepository.ExistsByEmailAsync(
                request.Email,
                excludedMemberId: null,
                cancellationToken);

            if (emailExists)
                throw new InvalidOperationException("A member with the same email already exists.");

            var membershipNumberExists = await _memberRepository.ExistsByMembershipNumberAsync(
                request.MembershipNumber,
                excludedMemberId: null,
                cancellationToken);

            if (membershipNumberExists)
                throw new InvalidOperationException("A member with the same membership number already exists.");

            var member = new Member
            {
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim(),
                PhoneNumber = request.PhoneNumber?.Trim(),
                Address = request.Address?.Trim(),
                MembershipNumber = request.MembershipNumber.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _memberRepository.AddAsync(member, cancellationToken);
            await _memberRepository.SaveChangesAsync(cancellationToken);

            await _activityLogger.LogAsync(
                ActivityAction.CreateMember,
                nameof(Member),
                member.Id.ToString(),
                $"Member '{member.FullName}' was created.",
                cancellationToken);

            return MapToResponse(member);
        }

        public async Task<MemberResponse> UpdateAsync(
            int id,
            UpdateMemberRequest request,
            CancellationToken cancellationToken = default)
        {
            await _updateValidator.ValidateAndThrowAsync(request, cancellationToken);

            var member = await _memberRepository.GetByIdAsync(id, cancellationToken);

            if (member is null)
                throw new InvalidOperationException("Member was not found.");

            var emailExists = await _memberRepository.ExistsByEmailAsync(
                request.Email,
                excludedMemberId: id,
                cancellationToken);

            if (emailExists)
                throw new InvalidOperationException("Another member with the same email already exists.");

            var membershipNumberExists = await _memberRepository.ExistsByMembershipNumberAsync(
                request.MembershipNumber,
                excludedMemberId: id,
                cancellationToken);

            if (membershipNumberExists)
                throw new InvalidOperationException("Another member with the same membership number already exists.");

            member.FullName = request.FullName.Trim();
            member.Email = request.Email.Trim();
            member.PhoneNumber = request.PhoneNumber?.Trim();
            member.Address = request.Address?.Trim();
            member.MembershipNumber = request.MembershipNumber.Trim();
            member.IsActive = request.IsActive;
            member.UpdatedAt = DateTime.UtcNow;

            await _memberRepository.SaveChangesAsync(cancellationToken);

            await _activityLogger.LogAsync(
                ActivityAction.UpdateMember,
                nameof(Member),
                member.Id.ToString(),
                $"Member '{member.FullName}' was updated.",
                cancellationToken);

            return MapToResponse(member);
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var member = await _memberRepository.GetByIdAsync(id, cancellationToken);

            if (member is null)
                throw new InvalidOperationException("Member was not found.");

            var hasActiveBorrowings = await _memberRepository.HasActiveBorrowingsAsync(
                id,
                cancellationToken);

            if (hasActiveBorrowings)
                throw new InvalidOperationException("Cannot delete this member because they have active borrowings.");

            _memberRepository.Remove(member);
            await _memberRepository.SaveChangesAsync(cancellationToken);

            await _activityLogger.LogAsync(
                ActivityAction.DeleteMember,
                nameof(Member),
                id.ToString(),
                $"Member '{member.FullName}' was deleted.",
                cancellationToken);
        }

        public async Task ActivateAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var member = await _memberRepository.GetByIdAsync(id, cancellationToken);

            if (member is null)
                throw new InvalidOperationException("Member was not found.");

            member.Activate();

            await _memberRepository.SaveChangesAsync(cancellationToken);

            await _activityLogger.LogAsync(
                ActivityAction.UpdateMember,
                nameof(Member),
                member.Id.ToString(),
                $"Member '{member.FullName}' was activated.",
                cancellationToken);
        }

        public async Task DeactivateAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var member = await _memberRepository.GetByIdAsync(id, cancellationToken);

            if (member is null)
                throw new InvalidOperationException("Member was not found.");

            member.Deactivate();

            await _memberRepository.SaveChangesAsync(cancellationToken);

            await _activityLogger.LogAsync(
                ActivityAction.UpdateMember,
                nameof(Member),
                member.Id.ToString(),
                $"Member '{member.FullName}' was deactivated.",
                cancellationToken);
        }

        private static MemberResponse MapToResponse(Member member)
        {
            return new MemberResponse
            {
                Id = member.Id,
                FullName = member.FullName,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                Address = member.Address,
                MembershipNumber = member.MembershipNumber,
                IsActive = member.IsActive,
                CreatedAt = member.CreatedAt,
                UpdatedAt = member.UpdatedAt
            };
        }
    }
}
