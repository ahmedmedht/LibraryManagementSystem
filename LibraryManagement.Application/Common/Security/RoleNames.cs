using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Common.Security
{
    public static class RoleNames
    {
        public const string Administrator = "Administrator";

        public const string Librarian = "Librarian";

        public const string Staff = "Staff";

        public const string AdministratorOrLibrarian = Administrator + "," + Librarian;

        public const string AllSystemUsers = Administrator + "," + Librarian + "," + Staff;
    }
}
