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
    public class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
    {
        public void Configure(EntityTypeBuilder<BookAuthor> builder)
        {
            builder.ToTable("BookAuthors");

            builder.HasKey(bookAuthor => new
            {
                bookAuthor.BookId,
                bookAuthor.AuthorId
            });

            builder.HasOne(bookAuthor => bookAuthor.Book)
                .WithMany(book => book.BookAuthors)
                .HasForeignKey(bookAuthor => bookAuthor.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bookAuthor => bookAuthor.Author)
                .WithMany(author => author.BookAuthors)
                .HasForeignKey(bookAuthor => bookAuthor.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
