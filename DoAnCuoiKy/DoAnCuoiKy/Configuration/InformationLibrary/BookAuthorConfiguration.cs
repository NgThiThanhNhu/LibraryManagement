using DoAnCuoiKy.Model.Entities.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
    {
        public void Configure(EntityTypeBuilder<BookAuthor> builder)
        {
            builder.HasKey(ba => ba.Id);

            builder.Property(ba=>ba.Name)
                .HasMaxLength(256);

            //mối quan hệ với bảng book
            builder.HasMany(ba => ba.books)
                .WithOne(b => b.BookAuthor)
                .HasForeignKey(b => b.BookAuthorId);

           
        }
    }
}
