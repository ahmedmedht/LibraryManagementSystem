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
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(book => book.Id);

            builder.Property(book => book.Title)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(book => book.ISBN)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasIndex(book => book.ISBN)
                .IsUnique();

            builder.Property(book => book.PublicationYear)
                .IsRequired();

            builder.Property(book => book.Edition)
                .HasMaxLength(100);

            builder.Property(book => book.Summary)
                .HasMaxLength(2000);

            builder.Property(book => book.CoverImageUrl)
                .HasMaxLength(1000);

            builder.Property(book => book.Language)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(book => book.TotalCopies)
                .IsRequired();

            builder.Property(book => book.AvailableCopies)
                .IsRequired();

            builder.Property(book => book.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(book => book.CreatedAt)
                .IsRequired();

            builder.Property(book => book.UpdatedAt);

            builder.HasOne(book => book.Publisher)
                .WithMany(publisher => publisher.Books)
                .HasForeignKey(book => book.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
