using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Request
{
    public class FineRequest
    {
        public Guid BorrowingDetailId { get; set; }
        public FineReason? fineReason { get; set; }


    }
}
