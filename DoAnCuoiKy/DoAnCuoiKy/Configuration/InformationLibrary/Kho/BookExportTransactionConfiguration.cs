using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary.Kho
{
    public class BookExportTransactionConfiguration : IEntityTypeConfiguration<BookExportTransaction>
    {
        public void Configure(EntityTypeBuilder<BookExportTransaction> builder)
        {
            builder.HasKey(ep => ep.Id);
            builder.Property(ep => ep.ExportReason)
                .HasConversion<byte>();
        }
    }
}
