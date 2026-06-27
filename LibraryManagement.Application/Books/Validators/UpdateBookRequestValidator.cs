using FluentValidation;
using LibraryManagement.Application.Books.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Books.Validators
{
    public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty()
                .WithMessage("Book title is required.")
                .MaximumLength(250)
                .WithMessage("Book title must not exceed 250 characters.");

            RuleFor(request => request.ISBN)
                .NotEmpty()
                .WithMessage("ISBN is required.")
                .MaximumLength(30)
                .WithMessage("ISBN must not exceed 30 characters.");

            RuleFor(request => request.PublicationYear)
                .InclusiveBetween(1000, DateTime.UtcNow.Year)
                .WithMessage("Publication year is invalid.");

            RuleFor(request => request.Edition)
                .MaximumLength(100)
                .WithMessage("Edition must not exceed 100 characters.");

            RuleFor(request => request.Summary)
                .MaximumLength(2000)
                .WithMessage("Summary must not exceed 2000 characters.");

            RuleFor(request => request.CoverImageUrl)
                .MaximumLength(1000)
                .WithMessage("Cover image URL must not exceed 1000 characters.");

            RuleFor(request => request.Language)
                .NotEmpty()
                .WithMessage("Language is required.")
                .MaximumLength(100)
                .WithMessage("Language must not exceed 100 characters.");

            RuleFor(request => request.PublisherId)
                .GreaterThan(0)
                .WithMessage("Publisher id is required.");

            RuleFor(request => request.TotalCopies)
                .GreaterThan(0)
                .WithMessage("Total copies must be greater than zero.");

            RuleFor(request => request.AvailableCopies)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Available copies cannot be negative.");

            RuleFor(request => request)
                .Must(request => request.AvailableCopies <= request.TotalCopies)
                .WithMessage("Available copies cannot exceed total copies.");

            RuleFor(request => request.AuthorIds)
                .NotEmpty()
                .WithMessage("At least one author is required.");

            RuleFor(request => request.CategoryIds)
                .NotEmpty()
                .WithMessage("At least one category is required.");
        }
    }
}
