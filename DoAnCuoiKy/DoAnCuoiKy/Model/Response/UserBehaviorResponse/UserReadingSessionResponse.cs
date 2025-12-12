using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;

namespace DoAnCuoiKy.Model.Response.UserBehaviorResponse
{
    public class UserReadingSessionResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int DurationSeconds { get; set; }
        public int LastPageNumber { get; set; }
        public int TotalPages { get; set; }
        public decimal ReadingProgress { get; set; }
        public bool IsCompleted { get; set; }
    }
}
