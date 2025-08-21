using DoAnCuoiKy.Model.Entities.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class BorrowingDetailResponse
    {
        public string BorrowingCode { get; set; }
        public string BookItemTitle { get; set; }
        public string UrlImage { get; set; }
        public string AuthorBookItem { get; set; }
        public string CategoryName { get; set; }
        public int QuantityStorage { get; set; }
        public DateTime ReturnedDate { get; set; } 
        public bool IsScheduled { get; set; }

    }
}
