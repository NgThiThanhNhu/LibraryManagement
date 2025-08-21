namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class BookPickupSchedule : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid BorrowingId { get; set; }
        public Borrowing borrowing { get; set; }
        public DateTime? ScheduledPickupDate { get; set; }
        public DateTime? ExpiredPickupDate { get; set; }   // Hạn chót để đến lấy sách
        public bool IsPickedUp { get; set; } = false;       // Đã đến lấy sách chưa
        public bool IsNotified { get; set; } = false; //thông báo gmail/sđt
        public DateTime NotificationTime { get; set; }
    }
}
