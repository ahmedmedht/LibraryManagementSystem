using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Enums
{
    public enum ActivityAction
    {
        Login = 1,

        CreateBook = 2,
        UpdateBook = 3,
        DeleteBook = 4,

        CreateAuthor = 5,
        UpdateAuthor = 6,
        DeleteAuthor = 7,

        CreatePublisher = 8,
        UpdatePublisher = 9,
        DeletePublisher = 10,

        CreateCategory = 11,
        UpdateCategory = 12,
        DeleteCategory = 13,

        CreateMember = 14,
        UpdateMember = 15,
        DeleteMember = 16,

        BorrowBook = 17,
        ReturnBook = 18,

        CreateUser = 19,
        UpdateUser = 20,
        DeleteUser = 21,
        ChangeUserRole = 22
    }
}
