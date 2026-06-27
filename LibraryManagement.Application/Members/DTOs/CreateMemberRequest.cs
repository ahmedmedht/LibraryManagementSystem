using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Members.DTOs
{
    public class CreateMemberRequest
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string MembershipNumber { get; set; } = string.Empty;
    }
}
