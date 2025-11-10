using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Enum.InformationLibrary.Kho;

namespace DoAnCuoiKy.Model.Request
{
    public class BookImportTransactionRequest
    {
        public int Quantity { get; set; }  // Số lượng nhập
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
