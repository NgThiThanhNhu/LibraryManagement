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
            //mối quan hệ giữa bookcart với user
            builder.HasOne(bc => bc.User)
                .WithMany(u => u.bookCartItems)
                .HasForeignKey(bc => bc.UserId);
            //mối quan hệ giữa bookcart và bookitem
            builder.HasOne(bc => bc.BookItem)
               .WithOne(i => i.BookCartItem)
               .HasForeignKey<BookCartItem>(bc => bc.BookItemId);
                
        }
    }
}
