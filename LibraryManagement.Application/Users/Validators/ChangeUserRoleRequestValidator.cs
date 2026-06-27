using FluentValidation;
using LibraryManagement.Application.Common.Security;
using LibraryManagement.Application.Users.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Users.Validators
{
    public class ChangeUserRoleRequestValidator : AbstractValidator<ChangeUserRoleRequest>
    {
        private static readonly string[] AllowedRoles =
        {
        RoleNames.Administrator,
        RoleNames.Librarian,
        RoleNames.Staff
    };

        public ChangeUserRoleRequestValidator()
        {
            RuleFor(request => request.Role)
                .NotEmpty()
                .WithMessage("Role is required.")
                .Must(role => AllowedRoles.Contains(role))
                .WithMessage("Role must be Administrator, Librarian, or Staff.");
        }
    }
}
