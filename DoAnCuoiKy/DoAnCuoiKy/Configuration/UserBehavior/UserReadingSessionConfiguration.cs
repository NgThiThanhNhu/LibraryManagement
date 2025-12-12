using DoAnCuoiKy.Model.Entities.UserBehavior;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.UserBehavior
{
    public class UserReadingSessionConfiguration : IEntityTypeConfiguration<UserReadingSession>
    {
        public void Configure(EntityTypeBuilder<UserReadingSession> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Librarian)
                .WithMany(l => l.userReadingSessions)
                .HasForeignKey(x => x.UserId);
            builder.HasOne(x=>x.Book)
                .WithMany(b=>b.userReadingSessions)
                .HasForeignKey(x => x.BookId);
            builder.Property(x => x.ReadingProgress)
                .HasPrecision(5, 2);
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_UserReadingSession_UserId");
            builder.HasIndex(x => x.BookId)
                .HasDatabaseName("IX_UserReadingSession_BookId");
            builder.HasIndex(x => new { x.UserId, x.BookId, x.StartTime })
                .HasDatabaseName("IX_UserReadingSession_Composite");
            builder.HasIndex(x => x.StartTime)
                .HasDatabaseName("IX_UserReadingSession_StartTime");
        }
    }
}
