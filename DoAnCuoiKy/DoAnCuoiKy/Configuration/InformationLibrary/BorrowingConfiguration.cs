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

            //với bảng user
            builder.HasOne(bo => bo.users)
                .WithMany(u => u.borrowings)
                .HasForeignKey(bo => bo.UserId);

            //với bảng libararian
           

          
        }
    }
}
