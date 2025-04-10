using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary.Kho
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(l => l.Id);

            //mối quan hê với ShelfSection
            builder.HasOne(l => l.ShelfSection)
                .WithMany(section => section.Locations)
                .HasForeignKey(l => l.ShelfSectionId);

            //mối quan hệ với bảng bookitem
            builder.HasMany(l => l.BookItems)
                .WithOne(i => i.Location)
                .HasForeignKey(i => i.LocationId);

            builder.Property(l => l.Description)
                .HasMaxLength(256);
        }

    }
}
