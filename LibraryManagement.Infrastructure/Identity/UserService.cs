using FluentValidation;
using LibraryManagement.Application.ActivityLogs.Interfaces;
using LibraryManagement.Application.Users.DTOs;
using LibraryManagement.Application.Users.Interfaces;
using LibraryManagement.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IActivityLogger _activityLogger;
        private readonly IValidator<CreateUserRequest> _createValidator;
        private readonly IValidator<UpdateUserRequest> _updateValidator;
        private readonly IValidator<ChangeUserRoleRequest> _changeRoleValidator;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IActivityLogger activityLogger,
            IValidator<CreateUserRequest> createValidator,
            IValidator<UpdateUserRequest> updateValidator,
            IValidator<ChangeUserRoleRequest> changeRoleValidator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _activityLogger = activityLogger;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _changeRoleValidator = changeRoleValidator;
        }

        public async Task<IReadOnlyList<UserResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var users = await _userManager.Users
                .OrderBy(user => user.FullName)
                .ToListAsync(cancellationToken);

            var responses = new List<UserResponse>();

            foreach (var user in users)
            {
                responses.Add(await MapToResponseAsync(user));
            }

            return responses;
        }

        public async Task<UserResponse?> GetByIdAsync(
            string id,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return null;

            return await MapToResponseAsync(user);
        }

        public async Task<UserResponse> CreateAsync(
            CreateUserRequest request,
            CancellationToken cancellationToken = default)
        {
            await _createValidator.ValidateAndThrowAsync(request, cancellationToken);

            var roleExists = await _roleManager.RoleExistsAsync(request.Role);

            if (!roleExists)
                throw new InvalidOperationException("Role was not found.");

            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser is not null)
                throw new InvalidOperationException("A user with the same email already exists.");

            var user = new ApplicationUser
            {
                UserName = request.Email.Trim(),
                Email = request.Email.Trim(),
                FullName = request.FullName.Trim(),
                EmailConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
                ThrowIdentityErrors(createResult);

            var roleResult = await _userManager.AddToRoleAsync(user, request.Role);

            if (!roleResult.Succeeded)
                ThrowIdentityErrors(roleResult);

            await _activityLogger.LogAsync(
                ActivityAction.CreateUser,
                nameof(ApplicationUser),
                user.Id,
                $"User '{user.Email}' was created with role '{request.Role}'.",
                cancellationToken);

            return await MapToResponseAsync(user);
        }

        public async Task<UserResponse> UpdateAsync(
            string id,
            UpdateUserRequest request,
            CancellationToken cancellationToken = default)
        {
            await _updateValidator.ValidateAndThrowAsync(request, cancellationToken);

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                throw new InvalidOperationException("User was not found.");

            user.FullName = request.FullName.Trim();
            user.PhoneNumber = request.PhoneNumber?.Trim();
            user.IsActive = request.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                ThrowIdentityErrors(updateResult);

            await _activityLogger.LogAsync(
                ActivityAction.UpdateUser,
                nameof(ApplicationUser),
                user.Id,
                $"User '{user.Email}' was updated.",
                cancellationToken);

            return await MapToResponseAsync(user);
        }

        public async Task DeleteAsync(
            string id,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                throw new InvalidOperationException("User was not found.");

            var deleteResult = await _userManager.DeleteAsync(user);

            if (!deleteResult.Succeeded)
                ThrowIdentityErrors(deleteResult);

            await _activityLogger.LogAsync(
                ActivityAction.DeleteUser,
                nameof(ApplicationUser),
                id,
                $"User '{user.Email}' was deleted.",
                cancellationToken);
        }

        public async Task<UserResponse> ChangeRoleAsync(
            string id,
            ChangeUserRoleRequest request,
            CancellationToken cancellationToken = default)
        {
            await _changeRoleValidator.ValidateAndThrowAsync(request, cancellationToken);

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                throw new InvalidOperationException("User was not found.");

            var roleExists = await _roleManager.RoleExistsAsync(request.Role);

            if (!roleExists)
                throw new InvalidOperationException("Role was not found.");

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
                ThrowIdentityErrors(removeResult);

            var addResult = await _userManager.AddToRoleAsync(user, request.Role);

            if (!addResult.Succeeded)
                ThrowIdentityErrors(addResult);

            user.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                ThrowIdentityErrors(updateResult);

            await _activityLogger.LogAsync(
                ActivityAction.ChangeUserRole,
                nameof(ApplicationUser),
                user.Id,
                $"User '{user.Email}' role changed to '{request.Role}'.",
                cancellationToken);

            return await MapToResponseAsync(user);
        }

        public async Task ActivateAsync(
            string id,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                throw new InvalidOperationException("User was not found.");

            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                ThrowIdentityErrors(updateResult);

            await _activityLogger.LogAsync(
                ActivityAction.UpdateUser,
                nameof(ApplicationUser),
                user.Id,
                $"User '{user.Email}' was activated.",
                cancellationToken);
        }

        public async Task DeactivateAsync(
            string id,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                throw new InvalidOperationException("User was not found.");

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                ThrowIdentityErrors(updateResult);

            await _activityLogger.LogAsync(
                ActivityAction.UpdateUser,
                nameof(ApplicationUser),
                user.Id,
                $"User '{user.Email}' was deactivated.",
                cancellationToken);
        }

        private async Task<UserResponse> MapToResponseAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                Roles = roles,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        private static void ThrowIdentityErrors(IdentityResult result)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(error => error.Description));

            throw new InvalidOperationException(errors);
        }
    }
}
