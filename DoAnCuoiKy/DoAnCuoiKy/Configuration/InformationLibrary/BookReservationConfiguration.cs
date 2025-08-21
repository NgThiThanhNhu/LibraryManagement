using DoAnCuoiKy.Model.Entities.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary
{
    public class BookReservationConfiguration : IEntityTypeConfiguration<BookReservation>
    {
        public void Configure(EntityTypeBuilder<BookReservation> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.ReservationDate);

            builder.Property(r => r.ExpireDate);

            //xây dựng mối quan hệ
            //với bảng user
          

         
            //với bảng bookitem
            builder.HasOne(r => r.BookItem)
                .WithMany(i => i.BookReservations)
                .HasForeignKey(r => r.BookItemId);

            

           
        }
    }
}
