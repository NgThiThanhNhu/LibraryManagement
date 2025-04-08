using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                //.IsRequired()
                .HasMaxLength(200);
           
           
            builder.Property(b => b.YearPublished);

            builder.Property(b => b.Quantity);

            //builder.Property(b => b.Status)
            //    .HasConversion<byte>();

            //thiết lập mối quan hệ
            builder.HasOne(b => b.Category)
                .WithMany(cg => cg.books)
                .HasForeignKey(b => b.CategoryId);
            //builder.HasMany(b => b.Reservations)
            //    .WithOne(r => r.book)
            //    .HasForeignKey()
          
        }
    }
}
