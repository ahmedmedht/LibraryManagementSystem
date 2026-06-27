using FluentValidation;
using LibraryManagement.Application.Users.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Users.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(request => request.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.")
                .MaximumLength(200)
                .WithMessage("Full name must not exceed 200 characters.");

            RuleFor(request => request.PhoneNumber)
                .MaximumLength(50)
                .WithMessage("Phone number must not exceed 50 characters.");
        }
    }
}
