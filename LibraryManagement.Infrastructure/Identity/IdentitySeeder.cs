using LibraryManagement.Application.Common.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await EnsureRoleAsync(roleManager, RoleNames.Administrator);
            await EnsureRoleAsync(roleManager, RoleNames.Librarian);
            await EnsureRoleAsync(roleManager, RoleNames.Staff);

            await EnsureUserAsync(
                userManager,
                email: "admin@library.com",
                fullName: "System Administrator",
                password: "Admin@123",
                role: RoleNames.Administrator);

            await EnsureUserAsync(
                userManager,
                email: "librarian@library.com",
                fullName: "Default Librarian",
                password: "Librarian@123",
                role: RoleNames.Librarian);

            await EnsureUserAsync(
                userManager,
                email: "staff@library.com",
                fullName: "Default Staff",
                password: "Staff@123",
                role: RoleNames.Staff);
        }

        private static async Task EnsureRoleAsync(
            RoleManager<IdentityRole> roleManager,
            string roleName)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                var result = await roleManager.CreateAsync(
                    new IdentityRole(roleName));

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        result.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Failed to create role '{roleName}'. Errors: {errors}");
                }
            }
        }

        private static async Task EnsureUserAsync(
            UserManager<ApplicationUser> userManager,
            string email,
            string fullName,
            string password,
            string role)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var createResult = await userManager.CreateAsync(user, password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        createResult.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Failed to create user '{email}'. Errors: {errors}");
                }
            }

            var isInRole = await userManager.IsInRoleAsync(user, role);

            if (!isInRole)
            {
                var roleResult = await userManager.AddToRoleAsync(user, role);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        roleResult.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Failed to add user '{email}' to role '{role}'. Errors: {errors}");
                }
            }
        }
    }
}
