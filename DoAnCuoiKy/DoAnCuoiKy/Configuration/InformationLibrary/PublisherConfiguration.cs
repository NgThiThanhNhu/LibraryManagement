using DoAnCuoiKy.Model.Entities.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p=>p.PublisherName)
                .HasMaxLength(256);

            builder.Property(p=>p.Address)
                .HasMaxLength(256);

            builder.Property(p=>p.Email)
                .HasMaxLength(256);

            builder.Property(p=>p.Phone)
                .HasMaxLength(10);

            //xây dựng mối quan hệ với bảng book
            builder.HasMany(p => p.Books)
                .WithOne(b => b.Publisher)
                .HasForeignKey(b => b.PublisherId);
        }
    }
}
