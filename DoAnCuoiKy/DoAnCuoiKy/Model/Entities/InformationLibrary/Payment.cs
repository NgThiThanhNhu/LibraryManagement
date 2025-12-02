using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class Payment : BaseEntity
    {
        public Guid Id { get; set; }
        public string? OrderId { get; set; }           // Mã thanh toán VNPay
        public string? VnpText { get; set; }
        public string? VnpResponseCode { get; set; }
        public PaymentType? PaymentType { get; set; } // VNPay, Cash...
        public string? TransactionNo { get; set; }
        public Guid? BorrowingId { get; set; }
        public Borrowing? Borrowing { get; set; }
        public decimal? BorrowAmount { get; set; }
        public string? vnpBankCode { get; set; }
    }

}
