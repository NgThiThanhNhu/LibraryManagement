using DoAnCuoiKy.Model.Entities.Notification;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class Borrowing : BaseEntity
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public int Duration { get; set; }
        public DateTime DueDate { get; set; } /*= CreateDate.AddDays(Duration);*/ //ngày hết hạn được tính dựa trên BorrowDate + Duration
        public BorrowingStatus? BorrowingStatus { get; set; }
        //mối quan hệ giữa borrowingDetail
        public Guid? LibrarianId { get; set; }
        public Librarian Librarian { get; set; }
        public Guid? UserId { get; set; }
        public bool isScheduled { get; set; } = false;
        public bool isReminded { get; set; } = false;
        public List<BorrowingDetail> borrowingDetails { get; set; }
       
        public BookPickupSchedule? BookPickupSchedule { get; set; }
        public List<NotificationToUser> notificationToUsers { get; set; }

    }
}
