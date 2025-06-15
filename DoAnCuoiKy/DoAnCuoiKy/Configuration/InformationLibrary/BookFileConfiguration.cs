using DoAnCuoiKy.Model.Entities.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class BookFileConfiguration : IEntityTypeConfiguration<BookFile>
    {
        public void Configure(EntityTypeBuilder<BookFile> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FileUrl)
                .HasMaxLength(500);
            builder.Property(x => x.PublicIdFile)
                .HasMaxLength(255);
            builder.Property(x => x.ImageUrl)
               .HasMaxLength(500);
            builder.Property(x => x.PublicIdImage)
                .HasMaxLength(255);
            //mối quan hệ với bảng book
            builder.HasOne(bf => bf.book)
                .WithMany(b => b.bookFiles)
                .HasForeignKey(bf => bf.BookId);
            builder.Property(bf => bf.bookFileType)
               .HasConversion<byte>()
               .HasMaxLength(20);


        }
    }
}
