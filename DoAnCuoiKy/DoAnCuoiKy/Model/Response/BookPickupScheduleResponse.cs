namespace DoAnCuoiKy.Model.Response
{
    public class BookPickupScheduleResponse
    {
        public Guid Id { get; set; }
        public Guid BorrowingId { get; set; }
        public DateTime ScheduledPickupDate { get; set; }
        public DateTime? ExpiredPickupDate { get; set; }  
        public bool IsPickedUp { get; set; }
        public string LibrarianName { get; set; }
        public string UserName { get; set; }
        public bool isScheduled { get; set; }
    }
}
