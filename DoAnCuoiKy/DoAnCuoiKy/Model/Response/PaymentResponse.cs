using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class PaymentResponse
    {
        public decimal BorrowAmount { get; set; }
        public PaymentType PaymentType { get; set; }
        public string? TransactionNo { get; set; }
        public string VnpText { get; set; }
        public string VnpResponseCode { get; set; }
        public DateTime CreateDate { get; set; }
        public string PaymentUrl { get; set; }
        public string vnpBankCode { get; set; }
    }
}
