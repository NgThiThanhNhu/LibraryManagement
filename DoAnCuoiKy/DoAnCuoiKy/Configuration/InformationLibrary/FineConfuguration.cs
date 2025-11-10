using DoAnCuoiKy.Model.Entities.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class FineConfuguration : IEntityTypeConfiguration<Fine>
    {
        public void Configure(EntityTypeBuilder<Fine> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(f => f.fineReason)
                .HasConversion<byte>();

            builder.Property(f => f.IssuedDate);

            builder.Property(f => f.IsPaid);
            builder.Property(f => f.DaysLate);
            builder.Property(f => f.FineRate)
                .HasColumnType("decimal(18,2)");

            //với bảng BorrowingDetail
            builder.HasOne(f => f.borrowingDetail)
                .WithMany(bod => bod.fines)
                .HasForeignKey(f => f.BorrowingDetailId);

            //với bảng librarian
            builder.HasOne(f => f.librarian)
                 .WithMany(l => l.fines)
                 .HasForeignKey(f => f.LibrarianId);


            
        }
    }
}
