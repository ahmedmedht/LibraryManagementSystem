using FluentValidation;
using LibraryManagement.Application.Borrowings.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Borrowings.Validators
{
    public class BorrowBookRequestValidator : AbstractValidator<BorrowBookRequest>
    {
        public BorrowBookRequestValidator()
        {
            RuleFor(request => request.BookId)
                .GreaterThan(0)
                .WithMessage("Book id is required.");

            RuleFor(request => request.MemberId)
                .GreaterThan(0)
                .WithMessage("Member id is required.");

            RuleFor(request => request.DueDate)
                .Must(dueDate => dueDate.Date > DateTime.UtcNow.Date)
                .WithMessage("Due date must be in the future.");
        }
    }
}
