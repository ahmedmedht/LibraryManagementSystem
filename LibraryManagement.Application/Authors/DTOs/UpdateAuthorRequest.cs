using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Authors.DTOs
{
    public class UpdateAuthorRequest
    {
        public string FullName { get; set; } = string.Empty;

        public string? Bio { get; set; }
    }
}
