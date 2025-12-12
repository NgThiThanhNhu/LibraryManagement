using DoAnCuoiKy.Model.Entities.UserBehavior;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.UserBehavior
{
    public class UserBookInteractionConfiguration : IEntityTypeConfiguration<UserBookInteraction>
    {
        public void Configure(EntityTypeBuilder<UserBookInteraction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Librarian)
                .WithMany(l => l.userBookInteractions)
                .HasForeignKey(x => x.UserId);
            builder.HasOne(x=>x.Book)
                .WithMany(b=>b.userBookInteractions)
                .HasForeignKey(x => x.BookId);
            builder.Property(x => x.InteractionType)
                .HasConversion<byte>();
            builder.Property(x => x.ReviewText)
                .HasMaxLength(2000);
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_UserBookInteraction_UserId");
            builder.HasIndex(x => x.BookId)
                .HasDatabaseName("IX_UserBookInteraction_BookId");
            builder.HasIndex(x => new { x.UserId, x.InteractionType })
                .HasDatabaseName("IX_UserBookInteraction_User_Type");
            builder.HasIndex(x => new { x.BookId, x.InteractionType, x.CreateDate })
                .HasDatabaseName("IX_UserBookInteraction_Book_Type_Date");
            builder.HasIndex(x => x.CreateDate)
                .HasDatabaseName("IX_UserBookInteraction_Date");
        }
    }
}
