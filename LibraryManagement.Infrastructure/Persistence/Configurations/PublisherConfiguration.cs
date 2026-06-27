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
    public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.ToTable("Publishers");

            builder.HasKey(publisher => publisher.Id);

            builder.Property(publisher => publisher.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(publisher => publisher.Address)
                .HasMaxLength(500);

            builder.Property(publisher => publisher.Website)
                .HasMaxLength(500);

            builder.Property(publisher => publisher.CreatedAt)
                .IsRequired();

            builder.Property(publisher => publisher.UpdatedAt);
        }
    }
}
