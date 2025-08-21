using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class BorrowingUserResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public int Duration { get; set; }
        public DateTime DueDate { get; set; }
        public string? Status { get; set; }
        public string ReplyDate { get; set; }
        public DateTime CreateDate { get; set; }
       
    }
}
