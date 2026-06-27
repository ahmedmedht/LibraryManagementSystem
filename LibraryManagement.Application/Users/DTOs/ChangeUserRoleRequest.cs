using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Users.DTOs
{
    public class ChangeUserRoleRequest
    {
        public string Role { get; set; } = string.Empty;
    }
}
