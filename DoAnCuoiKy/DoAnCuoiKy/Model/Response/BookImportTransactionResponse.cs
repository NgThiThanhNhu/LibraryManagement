using DoAnCuoiKy.Model.Enum.InformationLibrary.Kho;

namespace DoAnCuoiKy.Model.Response
{
    public class BookImportTransactionResponse
    {
        public Guid? Id { get; set; }
        public Guid? BookId { get; set; }
        public string? BookTitle { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
