using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Categories.DTOs
{
    public class CategoryResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int? ParentCategoryId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
