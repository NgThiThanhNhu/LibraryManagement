using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using System.ComponentModel.DataAnnotations;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class Book : BaseEntity
    {
       
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }

        public Guid PublisherId { get; set; }
        public Publisher Publisher { get; set; }
       
        public int? YearPublished { get; set; }
        public int? Quantity { get; set; } //số lượng sách nhập
        //public BookStatus? Status { get; set; }

        public Guid? CategoryId { get; set; }
        public BookCategory? Category { get; set; }
        
        //public List<BorrowingDetail> borrowingDetails { get; set; }
        //public List<BookReservation>? Reservations { get; set; }
        public Guid? BookChapterId { get; set; }
        public BookChapter? BookChapter { get; set; }
        public List<BookItem>? bookItems { get; set; }

       
    }
}
