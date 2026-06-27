using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Publishers.DTOs
{
    public class CreatePublisherRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? Website { get; set; }
    }
}
