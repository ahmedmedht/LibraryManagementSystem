using FluentValidation;
using LibraryManagement.Application.Authors.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Authors.Validators
{
    public class UpdateAuthorRequestValidator : AbstractValidator<UpdateAuthorRequest>
    {
        public UpdateAuthorRequestValidator()
        {
            RuleFor(request => request.FullName)
                .NotEmpty()
                .WithMessage("Author name is required.")
                .MaximumLength(200)
                .WithMessage("Author name must not exceed 200 characters.");

            RuleFor(request => request.Bio)
                .MaximumLength(2000)
                .WithMessage("Author bio must not exceed 2000 characters.");
        }
    }
}
