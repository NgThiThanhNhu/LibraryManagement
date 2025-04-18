using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAnCuoiKy.Configuration.InformationLibrary.Kho
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(room => room.Id);
            builder.Property(room => room.RoomName)
                .HasMaxLength(125);
            //mối quan hệ với Floor
            builder.HasOne(room => room.Floor)
                .WithMany(f => f.Rooms)
                .HasForeignKey(room => room.FloorId);
        }
    }
}
