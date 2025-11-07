using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class BorrowingDetailForFineResponse
    {
        public Guid BorrowingDetailId { get; set; }
        public string BookTitle { get; set; }
        public float UnitPrice { get; set; }
        public List<FineResponse> fineResponses { get; set; }
        public DateTime ReturnedDate { get; set; }
        public bool? IsFined { get; set; }
    }
}
