using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary.Kho
{
    public class ShelfConfiguration : IEntityTypeConfiguration<Shelf>
    {
        public void Configure(EntityTypeBuilder<Shelf> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s=>s.ShelfName)
                .HasMaxLength(128);
            builder.Property(s => s.NumberOfSections);
            //xây dựng mối quan hệ với bảng BookShelf
            builder.HasOne(s => s.Bookshelf)
                .WithMany(bs => bs.Shelves)
                .HasForeignKey(s => s.BookshelfId);
        }
    }
}
