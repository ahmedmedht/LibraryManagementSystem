using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Auth.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public IList<string> Roles { get; set; } = new List<string>();
    }
}
