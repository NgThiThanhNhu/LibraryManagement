using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Request
{
    public class PaymentRequest
    {
        public decimal BorrowAmount { get; set; }
        public PaymentType PaymentType { get; set; }
        public string VnpText { get; set; }
        public Guid BorrowingId { get; set; }
    }
}
