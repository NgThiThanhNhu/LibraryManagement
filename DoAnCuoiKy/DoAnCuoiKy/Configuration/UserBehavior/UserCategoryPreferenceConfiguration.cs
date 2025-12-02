using DoAnCuoiKy.Model.Entities.UserBehavior;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.UserBehavior
{
    public class UserCategoryPreferenceConfiguration : IEntityTypeConfiguration<UserCategoryPreference>
    {
        public void Configure(EntityTypeBuilder<UserCategoryPreference> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.BookCategory)
                .WithMany(c => c.userCategoryPreferences)
                .HasForeignKey(x => x.CategoryId);
            builder.HasOne(x => x.Librarian)
                .WithMany(l => l.categoryPreference)
                .HasForeignKey(x => x.UserId);
            builder.HasIndex(e => new { e.UserId, e.CategoryId })
                .IsUnique()
                .HasDatabaseName("IX_UserCategoryPreference_User_Category");
            builder.HasIndex(e => new { e.UserId, e.PreferenceScore })
                .HasDatabaseName("IX_UserCategoryPreference_User_Score");
            builder.Property(e => e.PreferenceScore)
                .HasPrecision(5, 2);
        }
    }
}
