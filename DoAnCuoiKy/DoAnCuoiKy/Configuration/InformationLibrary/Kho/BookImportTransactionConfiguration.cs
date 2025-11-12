using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary.Kho
{
    public class BookImportTransactionConfiguration : IEntityTypeConfiguration<BookImportTransaction>
    {
        public void Configure(EntityTypeBuilder<BookImportTransaction> builder)
        {
            builder.HasKey(ip => ip.Id);
            builder.Property(ip => ip.Quantity);
            builder.Property(ip => ip.UnitPrice)
                .HasColumnType("decimal(18,2)");
            builder.Property(ip => ip.TotalPrice)
                .HasColumnType("decimal(18,2)");
            builder.Property(ip => ip.TransactionType)
                .HasConversion<byte>();
        }
    }
}
