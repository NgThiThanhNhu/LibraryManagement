using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Entities.Notification
{
    public class NotificationToUser : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Librarian librarian { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public NotificationType NotificationType { get; set; }
        public Guid BorrowingId { get; set; }
        public Borrowing borrowing { get; set; }
    }
}
