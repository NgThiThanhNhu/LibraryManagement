using DoAnCuoiKy.Model.Entities.UserBehavior;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.UserBehavior
{
    public class UserSearchHistoryConfiguration : IEntityTypeConfiguration<UserSearchHistory>
    {
        public void Configure(EntityTypeBuilder<UserSearchHistory> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Librarian)
                .WithMany(l => l.userSearchHistories)
                .HasForeignKey(x => x.UserId);
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_UserSearchHistory_UserId");
            builder.HasIndex(x => x.SearchKeyword)
                .HasDatabaseName("IX_UserSearchHistory_Keyword");
            builder.HasIndex(x => x.CreateDate)
                .HasDatabaseName("IX_UserSearchHistory_SearchedAt");
            builder.HasIndex(x => new { x.UserId, x.CreateDate })
                .HasDatabaseName("IX_UserSearchHistory_User_Date");
            builder.Property(x => x.SearchKeyword)
                .HasMaxLength(500)
                .IsRequired();
            builder.Property(x => x.SearchType)
                .HasMaxLength(50);
            builder.Property(x => x.IpAddress)
                .HasMaxLength(50);
        }
    }
}
