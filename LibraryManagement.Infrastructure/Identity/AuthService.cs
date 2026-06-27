using LibraryManagement.Application.ActivityLogs.Interfaces;
using LibraryManagement.Application.Auth.DTOs;
using LibraryManagement.Application.Auth.Interfaces;
using LibraryManagement.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IActivityLogger _activityLogger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IJwtTokenService jwtTokenService,
            IActivityLogger activityLogger)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _activityLogger = activityLogger;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("This user account is inactive.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(
                user,
                request.Password);

            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);

            var token = await _jwtTokenService.GenerateTokenAsync(
                user.Id,
                user.Email ?? string.Empty,
                user.FullName,
                roles);

            await _activityLogger.LogAsync(
                user.Id,
                ActivityAction.Login,
                nameof(ApplicationUser),
                user.Id,
                $"User '{user.Email}' logged in.");

            return new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Roles = roles
            };
        }
    }
}
