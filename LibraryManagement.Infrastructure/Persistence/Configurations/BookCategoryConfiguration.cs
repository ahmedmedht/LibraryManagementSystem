using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Persistence.Configurations
{
    public class BookCategoryConfiguration : IEntityTypeConfiguration<BookCategory>
    {
        public void Configure(EntityTypeBuilder<BookCategory> builder)
        {
            builder.ToTable("BookCategories");

            builder.HasKey(bookCategory => new
            {
                bookCategory.BookId,
                bookCategory.CategoryId
            });

            builder.HasOne(bookCategory => bookCategory.Book)
                .WithMany(book => book.BookCategories)
                .HasForeignKey(bookCategory => bookCategory.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bookCategory => bookCategory.Category)
                .WithMany(category => category.BookCategories)
                .HasForeignKey(bookCategory => bookCategory.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
