using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Request
{
    public class NotificationToUserRequest
    {
        public Guid BorrowingId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
