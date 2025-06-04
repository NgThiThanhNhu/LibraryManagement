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
                .HasMaxLength(200);

            builder.Property(b => b.YearPublished);

            builder.Property(b => b.Quantity);

            builder.Property(b => b.UnitPrice);

            builder.Property(b => b.TotalPrice);
            
            //thiết lập mối quan hệ với bảng category
            builder.HasOne(b => b.Category)
                .WithMany(cg => cg.books)
                .HasForeignKey(b => b.CategoryId);
            
            //thiết lập mối quan hệ với bảng bookimporttransaction
            builder.HasMany(b => b.ImportTransactions)
                .WithOne(ip => ip.book)
                .HasForeignKey(ip => ip.BookId);
          
        }
    }
}
