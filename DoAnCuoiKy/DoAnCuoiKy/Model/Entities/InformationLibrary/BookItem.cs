using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class BookItem : BaseEntity
    {
        public Guid? Id { get; set; }
        public string BarCode { get; set; }
        public BookStatus? BookStatus { get; set; }
        public Guid? BookId { get; set; }
        public Book? Book { get; set; }
        public Guid? LocationId { get; set; }
        public Location? Location { get; set; }
        
        public List<BookReservation>? BookReservations { get; set; }
        public List<BorrowingDetail>? borrowingDetails { get; set; }
        public Guid? ExportTransactionId { get; set; }
        public BookExportTransaction? BookExportTransaction { get; set; }
       
       


    }
}
