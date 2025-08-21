using DoAnCuoiKy.Model.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class NotificationToUserConfiguration : IEntityTypeConfiguration<NotificationToUser>
    {
        public void Configure(EntityTypeBuilder<NotificationToUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.librarian)
                .WithMany(l => l.notifications)
                .HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.borrowing)
                .WithMany(b => b.notificationToUsers)
                .HasForeignKey(x => x.BorrowingId);
            builder.Property(x => x.Title);
            builder.Property(x => x.Message);
            builder.Property(x => x.IsRead);
            builder.Property(x => x.NotificationType)
                .HasConversion<byte>();

        }
    }
}
