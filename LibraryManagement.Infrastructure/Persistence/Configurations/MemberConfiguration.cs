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
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("Members");

            builder.HasKey(member => member.Id);

            builder.Property(member => member.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(member => member.Email)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasIndex(member => member.Email)
                .IsUnique();

            builder.Property(member => member.PhoneNumber)
                .HasMaxLength(50);

            builder.Property(member => member.Address)
                .HasMaxLength(500);

            builder.Property(member => member.MembershipNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(member => member.MembershipNumber)
                .IsUnique();

            builder.Property(member => member.IsActive)
                .IsRequired();

            builder.Property(member => member.CreatedAt)
                .IsRequired();

            builder.Property(member => member.UpdatedAt);
        }
    }
}
