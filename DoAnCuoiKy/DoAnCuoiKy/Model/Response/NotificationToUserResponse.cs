using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class NotificationToUserResponse
    {
        public Guid NotiId { get; set; }
        public Guid BorrowingId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        
        public Guid UserId { get; set; }
        
        public string BorrowingCode { get; set; }
        public DateTime SendTime { get; set; }
        public bool IsRead { get; set; }
    }
}
