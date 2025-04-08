

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

            //builder.Property(i=>i.Title)
            //    .HasMaxLength(256);

            //builder.Property(i=>i.Author)
            //    .HasMaxLength(256);

            //builder.Property(i=>i.Publisher)
            //    .HasMaxLength(256);

            //builder.Property(i => i.YearPublished);

            //builder.Property(i => i.Quantity);

            builder.Property(i => i.BookStatus)
                .HasConversion<byte>();

            //với bảng book
            builder.HasOne(i => i.Book)
                .WithMany(b => b.bookItems)
                .HasForeignKey(i => i.BookId);
          
        }
    }
}
