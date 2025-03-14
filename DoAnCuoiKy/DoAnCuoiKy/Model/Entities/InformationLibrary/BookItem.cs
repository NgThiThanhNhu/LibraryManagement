using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class BookItem : BaseEntity
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public int? YearPublished { get; set; }
        public int? Quantity { get; set; }
        public BookStatus? BookStatus { get; set; }
        public Guid? BookId { get; set; }
        public Book? Book { get; set; }
        public Guid? BookCategoryId { get; set; }
        public BookCategory? Category { get; set; }
        public Guid? BookChapterId { get; set; }
        public BookChapter? BookChapter { get; set; }
        
        public List<BookReservation>? BookReservations { get; set; }
        public List<BorrowingDetail>? borrowingDetails { get; set; }
       


    }
}
