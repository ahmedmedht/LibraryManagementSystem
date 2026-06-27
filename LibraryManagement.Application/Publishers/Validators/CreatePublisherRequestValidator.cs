using FluentValidation;
using LibraryManagement.Application.Publishers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Publishers.Validators
{
    public class CreatePublisherRequestValidator : AbstractValidator<CreatePublisherRequest>
    {
        public CreatePublisherRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .WithMessage("Publisher name is required.")
                .MaximumLength(200)
                .WithMessage("Publisher name must not exceed 200 characters.");

            RuleFor(request => request.Address)
                .MaximumLength(500)
                .WithMessage("Publisher address must not exceed 500 characters.");

            RuleFor(request => request.Website)
                .MaximumLength(500)
                .WithMessage("Publisher website must not exceed 500 characters.");
        }
    }
}
