using LibraryManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Books.DTOs
{
    public class BookResponse
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;

        public int PublicationYear { get; set; }

        public string? Edition { get; set; }

        public string? Summary { get; set; }

        public string? CoverImageUrl { get; set; }

        public string Language { get; set; } = string.Empty;

        public int PublisherId { get; set; }

        public string PublisherName { get; set; } = string.Empty;

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        public BookStatus Status { get; set; }

        public List<string> Authors { get; set; } = new();

        public List<string> Categories { get; set; } = new();

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
