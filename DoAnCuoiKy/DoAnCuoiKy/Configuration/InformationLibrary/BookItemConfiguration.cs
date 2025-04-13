

using DoAnCuoiKy.Model.Entities.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class BookItemConfiguration : IEntityTypeConfiguration<BookItem>
    {
        public void Configure(EntityTypeBuilder<BookItem> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.BarCode)
                .HasMaxLength(20);

            builder.Property(i => i.BookStatus)
                .HasConversion<byte>();

            //với bảng book
            builder.HasOne(i => i.Book)
                .WithMany(b => b.bookItems)
                .HasForeignKey(i => i.BookId);

            //mối quan hệ với bảng exporttransaction
            builder.HasOne(i => i.BookExportTransaction)
                .WithMany(ep => ep.bookItems)
                .HasForeignKey(i => i.ExportTransactionId);
          
        }
    }
}
