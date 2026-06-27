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
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Authors");

            builder.HasKey(author => author.Id);

            builder.Property(author => author.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(author => author.Bio)
                .HasMaxLength(2000);

            builder.Property(author => author.CreatedAt)
                .IsRequired();

            builder.Property(author => author.UpdatedAt);
        }
    }
}
