using FluentValidation;
using LibraryManagement.Application.Auth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Auth.Validators
{

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(request => request.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Email format is invalid.");

            RuleFor(request => request.Password)
                .NotEmpty()
                .WithMessage("Password is required.");
        }
    }
}
