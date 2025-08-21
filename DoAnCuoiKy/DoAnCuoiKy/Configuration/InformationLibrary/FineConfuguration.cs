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
                .HasColumnType("float");

            builder.Property(f => f.fineReason)
                .HasConversion<byte>();

            builder.Property(f => f.IssuedDate);

            builder.Property(f => f.IsPaid);


            //với bảng BorrowingDetail
            builder.HasOne(f => f.borrowingDetail)
                .WithMany(bod => bod.fines)
                .HasForeignKey(f => f.BorrowingDetailId);

            //với bảng librarian
           


            
        }
    }
}
