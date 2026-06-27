using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Categories.DTOs
{
    public class CategoryTreeResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<CategoryTreeResponse> Children { get; set; } = new();
    }
}
