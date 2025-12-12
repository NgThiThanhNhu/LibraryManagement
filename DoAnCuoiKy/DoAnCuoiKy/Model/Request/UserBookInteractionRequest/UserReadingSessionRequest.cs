namespace DoAnCuoiKy.Model.Request.UserBookInteractionRequest
{
    public class UserReadingSessionRequest
    {
        public Guid BookId { get; set; }
        public int LastPageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
