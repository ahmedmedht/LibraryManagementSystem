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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(category => category.Id);

            builder.Property(category => category.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(category => category.CreatedAt)
                .IsRequired();

            builder.Property(category => category.UpdatedAt);

            builder.HasOne(category => category.ParentCategory)
                .WithMany(category => category.SubCategories)
                .HasForeignKey(category => category.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
