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
    public class BorrowingTransactionConfiguration : IEntityTypeConfiguration<BorrowingTransaction>
    {
        public void Configure(EntityTypeBuilder<BorrowingTransaction> builder)
        {
            builder.ToTable("BorrowingTransactions");

            builder.HasKey(transaction => transaction.Id);

            builder.Property(transaction => transaction.BorrowedAt)
                .IsRequired();

            builder.Property(transaction => transaction.DueDate)
                .IsRequired();

            builder.Property(transaction => transaction.ReturnedAt);

            builder.Property(transaction => transaction.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(transaction => transaction.BorrowedByUserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(transaction => transaction.ReturnedByUserId)
                .HasMaxLength(450);

            builder.Property(transaction => transaction.CreatedAt)
                .IsRequired();

            builder.Property(transaction => transaction.UpdatedAt);

            builder.HasOne(transaction => transaction.Book)
                .WithMany(book => book.BorrowingTransactions)
                .HasForeignKey(transaction => transaction.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(transaction => transaction.Member)
                .WithMany(member => member.BorrowingTransactions)
                .HasForeignKey(transaction => transaction.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
