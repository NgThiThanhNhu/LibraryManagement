using DoAnCuoiKy.Model.Entities.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PaymentType)
                .HasConversion<byte>();
            builder.Property(x => x.TransactionNo);
            builder.Property(x => x.VnpText);
            builder.Property(x=>x.VnpResponseCode);
            builder.HasOne(x => x.Borrowing)
                .WithOne(y => y.Payment)
                .HasForeignKey<Payment>(x => x.BorrowingId);
            builder.Property(x => x.BorrowAmount)
                .HasColumnType("decimal(18,2)");
            builder.Property(x => x.OrderId)
                .HasMaxLength(50);
        }
    }
}
