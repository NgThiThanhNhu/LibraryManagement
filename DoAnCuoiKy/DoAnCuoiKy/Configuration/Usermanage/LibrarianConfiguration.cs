using DoAnCuoiKy.Model.Entities.Usermanage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.Usermanage
{
    public class LibrarianConfiguration : IEntityTypeConfiguration<Librarian>
    {
        public void Configure(EntityTypeBuilder<Librarian> builder)
        {
            builder.HasKey(lan => lan.Id);

            builder.Property(lan => lan.Name)
                .HasMaxLength(200);

            builder.Property(lan => lan.Email)
                .HasMaxLength(100);

            builder.Property(lan => lan.Phone)
                .HasMaxLength(10);

            builder.Property(lan => lan.Password)
                .HasMaxLength(255);

            //với bảng Role quan hệ nhiều nhiều
            builder.HasOne(lan => lan.Role)
                .WithMany(rl => rl.librarians)
                .HasForeignKey(lan => lan.RoleId);
        }
    }
}
