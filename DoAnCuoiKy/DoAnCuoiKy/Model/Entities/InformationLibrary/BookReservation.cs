using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class BookReservation : BaseEntity
    {
        public Guid? Id { get; set; }
        public DateTime? ReservationDate { get; set; } = DateTime.Now;
        public DateTime? ExpireDate { get; set; }
        public BookReservationStatus? Status { get; set; }
        //1 lượt đặt trước chỉ thuộc về một user
        public Guid? UserId { get; set; }
        public Users? users { get; set; }
        //public Guid? BookId { get; set; }
        //public Book? book { get; set; }
        public Guid? BookItemId { get; set; }
        public BookItem? BookItem { get; set; }
    }
}
