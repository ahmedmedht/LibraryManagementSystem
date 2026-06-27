using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Categories.DTOs
{
    public class UpdateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;

        public int? ParentCategoryId { get; set; }
    }
}
