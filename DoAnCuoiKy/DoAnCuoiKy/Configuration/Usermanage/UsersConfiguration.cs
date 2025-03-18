using DoAnCuoiKy.Model.Entities.Usermanage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.Usermanage
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .HasMaxLength(256);

            builder.Property(u => u.Phone)
                .HasMaxLength(10);

            builder.Property(u => u.Email)
                .HasMaxLength(100);

            builder.Property(u => u.Password)
                .HasMaxLength(255);

            builder.Property(u => u.RegistrationDate);

            builder.Property(u => u.IsActive);

            builder.HasOne(u => u.Role)
               .WithMany(rl => rl.users)
               .HasForeignKey(u => u.RoleId);

        }
    }
}
