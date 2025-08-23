namespace DoAnCuoiKy.Model.Response.KhoResponse
{
    public class BookExportTransactionResponse
    {
        public Guid Id { get; set; }
        public Guid BorrowingDetailId { get; set; }
        public string BookItemBarCode { get; set; }
        public string BookStatus { get; set; }
        public string ShelfSectionName { get; set; }
        public string ExportReason { get; set; }
        public string TransactionType { get; set; }
        public string ExportDate { get; set; } //createDate
        public string CreateBy { get; set; } //createuser
    }
}
