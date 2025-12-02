using DoAnCuoiKy.Model.Entities.UserBehavior;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.UserBehavior
{
    public class BookRecommendationConfiguration : IEntityTypeConfiguration<BookRecommendation>
    {
        public void Configure(EntityTypeBuilder<BookRecommendation> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Librarian)
                .WithMany(l => l.bookRecommendations)
                .HasForeignKey(x => x.UserId);
            builder.HasOne(x=>x.Book)
                .WithMany(b=>b.bookRecommendations)
                .HasForeignKey(b => b.BookId);
            builder.Property(x => x.Score)
               .HasPrecision(5, 2);
            builder.Property(x => x.Reason)
                .HasMaxLength(500);
            builder.Property(x => x.RecommendationType)
               .HasConversion<byte>();
            builder.HasIndex(x => x.CreateDate)
               .HasDatabaseName("IX_BookRecommendation_Date");
            builder.HasIndex(x => new { x.UserId, x.CreateDate })
                .HasDatabaseName("IX_BookRecommendation_User_Date");
            builder.HasIndex(x => new { x.UserId, x.Score })
                .HasDatabaseName("IX_BookRecommendation_User_Score");
        }
    }
}
