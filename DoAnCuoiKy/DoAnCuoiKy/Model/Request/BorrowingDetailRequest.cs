using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Request
{
    public class BorrowingDetailRequest
    {
       
        public DateTime ReturnedDate { get; set; }
        public BookStatus BookStatusBorrowingDetail { get; set; }
       
    }
}
