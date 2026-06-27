using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Users.DTOs
{
    public class UpdateUserRequest
    {
        public string FullName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }
    }
}
