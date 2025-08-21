using DoAnCuoiKy.Model.Entities.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class BookPickupScheduleConfiguration : IEntityTypeConfiguration<BookPickupSchedule>
    {
       
        public void Configure(EntityTypeBuilder<BookPickupSchedule> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.borrowing)
                .WithOne(c => c.BookPickupSchedule)
                .HasForeignKey<BookPickupSchedule>(x => x.BorrowingId);
            builder.Property(x => x.ScheduledPickupDate);
            builder.Property(x => x.ExpiredPickupDate);
            builder.Property(x => x.NotificationTime);
            
        }
    }
}
