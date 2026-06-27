using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Authors.DTOs
{
    public class AuthorResponse
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string? Bio { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
