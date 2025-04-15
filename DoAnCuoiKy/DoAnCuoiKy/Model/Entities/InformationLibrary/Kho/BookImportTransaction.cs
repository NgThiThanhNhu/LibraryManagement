using DoAnCuoiKy.Model.Enum.InformationLibrary.Kho;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary.Kho
{
    public class BookImportTransaction : BaseEntity
    {
        public Guid? Id { get; set; }
        public Guid? BookId { get; set; }
        public Book? book { get; set; }
        //public int Quantity { get; set; }  // Số lượng nhập
        ////public DateTime ImportDate { get; set; }
        //public float UnitPrice { get; set; }
        //public float TotalPrice { get; set; }
        public TransactionType? TransactionType { get; set; }
    }
}
