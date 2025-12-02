using DoAnCuoiKy.Model.Entities.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class BookCartConfiguration : IEntityTypeConfiguration<BookCart>
    {
        public void Configure(EntityTypeBuilder<BookCart> builder)
        {
            builder.HasKey(bc => bc.Id);
            builder.HasOne(bc => bc.Librarian)
                .WithMany(u => u.bookCarts)
                .HasForeignKey(bc => bc.UserId);
            builder.Property(bc => bc.CartStatus)
               .HasConversion<byte>()
               .HasMaxLength(20);
            builder.HasIndex(x => new { x.UserId, x.CartStatus })
                .HasDatabaseName("IX_BookCarts_UserId_Status");
        }
    }
}
