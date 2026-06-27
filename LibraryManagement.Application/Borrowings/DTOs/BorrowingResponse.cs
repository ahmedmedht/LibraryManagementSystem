using LibraryManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Borrowings.DTOs
{
    public class BorrowingResponse
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public string BookTitle { get; set; } = string.Empty;

        public int MemberId { get; set; }

        public string MemberName { get; set; } = string.Empty;

        public DateTime BorrowedAt { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnedAt { get; set; }

        public BorrowingStatus Status { get; set; }

        public string BorrowedByUserId { get; set; } = string.Empty;

        public string? ReturnedByUserId { get; set; }
    }
}
