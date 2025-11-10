using DoAnCuoiKy.Model.Entities.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class BorrowingConfiguration : IEntityTypeConfiguration<Borrowing>
    {
        public void Configure(EntityTypeBuilder<Borrowing> builder)
        {
            builder.HasKey(bo => bo.Id);
            builder.Property(bod => bod.BorrowingStatus)
                .HasConversion<byte>();
            builder.Property(bo => bo.DueDate);
            builder.Property(bo => bo.Code)
                .HasMaxLength(100);
            builder.Property(bo => bo.Duration);
        }
    }
}
