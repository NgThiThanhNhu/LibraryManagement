using DoAnCuoiKy.Model.Entities.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class BorrowingDetailConfiguration : IEntityTypeConfiguration<BorrowingDetail>
    {
        public void Configure(EntityTypeBuilder<BorrowingDetail> builder)
        {
            builder.HasKey(bod => bod.Id);

            builder.Property(bod => bod.ReturnedDate);

            builder.Property(bod => bod.Status)
                .HasConversion<byte>();

            //với bảng borrowing
            builder.HasOne(bod => bod.borrowing)
                .WithMany(bo => bo.borrowingDetails)
                .HasForeignKey(bod => bod.BorrowingId);

            //với bảng book
            //builder.HasOne(bod => bod.book)
            //    .WithMany(b => b.borrowingDetails)
            //    .HasForeignKey(bod => bod.BookId);
            //với bảng BookItem
            builder.HasOne(bod=>bod.bookItem)
                .WithMany(i=>i.borrowingDetails)
                .HasForeignKey(bod=>bod.BookItemId);
           

        }
    }
}
