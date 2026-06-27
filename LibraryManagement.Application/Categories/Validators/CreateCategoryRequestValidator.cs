using FluentValidation;
using LibraryManagement.Application.Categories.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Categories.Validators
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .MaximumLength(150)
                .WithMessage("Category name must not exceed 150 characters.");

            RuleFor(request => request.ParentCategoryId)
                .GreaterThan(0)
                .When(request => request.ParentCategoryId.HasValue)
                .WithMessage("Parent category id must be greater than zero.");
        }
    }
}
