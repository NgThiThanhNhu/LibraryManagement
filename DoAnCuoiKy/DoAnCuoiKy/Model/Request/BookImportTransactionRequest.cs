using DoAnCuoiKy.Model.Enum.InformationLibrary.Kho;

namespace DoAnCuoiKy.Model.Request
{
    public class BookImportTransactionRequest
    {
        public Guid? BookId { get; set; }
        public TransactionType? TransactionType { get; set; }
    }
}
