namespace DoAnCuoiKy.Model.Request
{
    public class BookPickupScheduleRequest
    {
        public Guid BorrowingId { get; set; }
        public DateTime scheduledPickupDate {  get; set; }  
        public DateTime ExpiredPickupDate { get; set; }
    }
}
