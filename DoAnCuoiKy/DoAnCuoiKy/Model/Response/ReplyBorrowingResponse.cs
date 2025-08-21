using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class ReplyBorrowingResponse
    {
        public Guid BorrowingId {  get; set; }
        public string BorrowingStatus { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        
    }
}
