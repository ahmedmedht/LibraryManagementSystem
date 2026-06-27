using LibraryManagement.Application.Users.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Users.Interfaces
{
    public interface IUserService
    {
        Task<IReadOnlyList<UserResponse>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<UserResponse?> GetByIdAsync(
            string id,
            CancellationToken cancellationToken = default);

        Task<UserResponse> CreateAsync(
            CreateUserRequest request,
            CancellationToken cancellationToken = default);

        Task<UserResponse> UpdateAsync(
            string id,
            UpdateUserRequest request,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            string id,
            CancellationToken cancellationToken = default);

        Task<UserResponse> ChangeRoleAsync(
            string id,
            ChangeUserRoleRequest request,
            CancellationToken cancellationToken = default);

        Task ActivateAsync(
            string id,
            CancellationToken cancellationToken = default);

        Task DeactivateAsync(
            string id,
            CancellationToken cancellationToken = default);
    }
}
