using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary.Kho
{
    public class BookShelfConfiguration : IEntityTypeConfiguration<BookShelf>
    {
        public void Configure(EntityTypeBuilder<BookShelf> builder)
        {
            builder.HasKey(bs => bs.Id);
            builder.Property(bs=>bs.BookShelfName)
                .HasMaxLength(256);
           
        }
    }
}
