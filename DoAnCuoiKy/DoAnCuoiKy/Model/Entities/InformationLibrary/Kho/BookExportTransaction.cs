using DoAnCuoiKy.Model.Enum.InformationLibrary.Kho;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary.Kho
{
    public class BookExportTransaction : BaseEntity
    {
        public Guid Id { get; set; }
        public List<BookItem> bookItems { get; set; }
        //public DateTime ExportDate { get; set; }
        public ExportReason ExportReason { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
