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
    public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            builder.ToTable("ActivityLogs");

            builder.HasKey(log => log.Id);

            builder.Property(log => log.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(log => log.Action)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(log => log.EntityName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(log => log.EntityId)
                .HasMaxLength(100);

            builder.Property(log => log.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(log => log.CreatedAt)
                .IsRequired();
        }
    }
}
