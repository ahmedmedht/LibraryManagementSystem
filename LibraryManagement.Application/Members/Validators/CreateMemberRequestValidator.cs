using FluentValidation;
using LibraryManagement.Application.Members.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Members.Validators
{
    public class CreateMemberRequestValidator : AbstractValidator<CreateMemberRequest>
    {
        public CreateMemberRequestValidator()
        {
            RuleFor(request => request.FullName)
                .NotEmpty()
                .WithMessage("Member name is required.")
                .MaximumLength(200)
                .WithMessage("Member name must not exceed 200 characters.");

            RuleFor(request => request.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Email format is invalid.")
                .MaximumLength(250)
                .WithMessage("Email must not exceed 250 characters.");

            RuleFor(request => request.PhoneNumber)
                .MaximumLength(50)
                .WithMessage("Phone number must not exceed 50 characters.");

            RuleFor(request => request.Address)
                .MaximumLength(500)
                .WithMessage("Address must not exceed 500 characters.");

            RuleFor(request => request.MembershipNumber)
                .NotEmpty()
                .WithMessage("Membership number is required.")
                .MaximumLength(50)
                .WithMessage("Membership number must not exceed 50 characters.");
        }
    }
}
