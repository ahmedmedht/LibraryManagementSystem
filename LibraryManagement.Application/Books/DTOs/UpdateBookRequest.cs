using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Books.DTOs
{
    public class UpdateBookRequest
    {
        public string Title { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;

        public int PublicationYear { get; set; }

        public string? Edition { get; set; }

        public string? Summary { get; set; }

        public string? CoverImageUrl { get; set; }

        public string Language { get; set; } = string.Empty;

        public int PublisherId { get; set; }

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        public List<int> AuthorIds { get; set; } = new();

        public List<int> CategoryIds { get; set; } = new();
    }
}
