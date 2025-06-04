using DoAnCuoiKy.Model.Entities.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class BookChapterConfiguration : IEntityTypeConfiguration<BookChapter>
    {
        public void Configure(EntityTypeBuilder<BookChapter> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.TitleChapter)
                .HasMaxLength(150);
            //với bảng book
            builder.HasMany(c => c.books)
                .WithOne(b => b.BookChapter)
                .HasForeignKey(b => b.BookChapterId);
           
            
        }
    }
}
