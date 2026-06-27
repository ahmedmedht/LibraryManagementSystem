using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Entities
{
    public class Publisher
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? Website { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
