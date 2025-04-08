using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary.Kho
{
    public class ShelfSectionConfiguration : IEntityTypeConfiguration<ShelfSection>
    {
        public void Configure(EntityTypeBuilder<ShelfSection> builder)
        {
            builder.HasKey(section => section.Id);
            builder.Property(section => section.SectionName)
                .HasMaxLength(128);
            builder.Property(section => section.Capacity);
            //mối quan hệ với kệ sách Shelf
            builder.HasOne(section => section.Shelf)
                .WithMany(s => s.Sections)
                .HasForeignKey(section => section.ShelfId);
                

        }
    }
}
