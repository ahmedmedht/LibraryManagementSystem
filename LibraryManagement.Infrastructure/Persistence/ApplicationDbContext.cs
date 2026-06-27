using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books => Set<Book>();

        public DbSet<Author> Authors => Set<Author>();

        public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>();

        public DbSet<Publisher> Publishers => Set<Publisher>();

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<BookCategory> BookCategories => Set<BookCategory>();

        public DbSet<Member> Members => Set<Member>();

        public DbSet<BorrowingTransaction> BorrowingTransactions => Set<BorrowingTransaction>();

        public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
