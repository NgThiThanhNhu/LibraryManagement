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
        public Guid? ShelfSectionId { get; set; } 
        public ShelfSection? ShelfSection { get; set; }
        public List<BookReservation>? BookReservations { get; set; }
        public List<BorrowingDetail>? borrowingDetails { get; set; }
    }
}
