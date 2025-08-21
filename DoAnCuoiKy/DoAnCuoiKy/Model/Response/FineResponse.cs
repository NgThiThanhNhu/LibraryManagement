using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class FineResponse
    {
        public Guid Id { get; set; }
        public float? Amount { get; set; }
        public FineReason? fineReason { get; set; }
        public DateTime? IssuedDate { get; set; }
        public Guid? BorrowingDetailId { get; set; }
        public bool? IsPaid { get; set; }
        public string userName { get; set; }
        public string librarianName { get; set; }
    }
}
