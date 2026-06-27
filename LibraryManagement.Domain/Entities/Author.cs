using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Entities
{

    public class Author
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string? Bio { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
