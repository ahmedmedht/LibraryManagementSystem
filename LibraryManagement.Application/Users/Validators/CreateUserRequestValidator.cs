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
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        private static readonly string[] AllowedRoles =
        {
        RoleNames.Administrator,
        RoleNames.Librarian,
        RoleNames.Staff
    };

        public CreateUserRequestValidator()
        {
            RuleFor(request => request.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.")
                .MaximumLength(200)
                .WithMessage("Full name must not exceed 200 characters.");

            RuleFor(request => request.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Email format is invalid.")
                .MaximumLength(250)
                .WithMessage("Email must not exceed 250 characters.");

            RuleFor(request => request.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters.");

            RuleFor(request => request.Role)
                .NotEmpty()
                .WithMessage("Role is required.")
                .Must(role => AllowedRoles.Contains(role))
                .WithMessage("Role must be Administrator, Librarian, or Staff.");
        }
    }
}
