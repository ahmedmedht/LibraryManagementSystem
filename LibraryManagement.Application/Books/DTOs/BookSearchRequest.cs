using LibraryManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Books.DTOs
{
    public class BookSearchRequest
    {
        public string? Name { get; set; }

        public string? Author { get; set; }

        public int? CategoryId { get; set; }

        public BookStatus? Status { get; set; }
    }
}
