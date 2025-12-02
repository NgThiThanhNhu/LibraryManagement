using DoAnCuoiKy.Model.Entities.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class BookCartItemConfiguration : IEntityTypeConfiguration<BookCartItem>
    {
        public void Configure(EntityTypeBuilder<BookCartItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Book)
                .WithMany(b => b.BookCartItems)
                .HasForeignKey(x=>x.BookId);
            builder.HasOne(x => x.BookCart)
                .WithMany(bc => bc.BookCartItems)
                .HasForeignKey(x => x.CartId);
            builder.Property(x => x.Quantity)
                .HasDefaultValue(1);
        }
    }
}
