using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class BorrowingDetail : BaseEntity
    {
        public Guid? Id { get; set; }
        public DateTime BorrowDate { get; set; }
        public int Duration { get; set; }
        public DateTime DueDate => BorrowDate.AddDays(Duration); //ngày hết hạn được tính dựa trên BorrowDate + Duration
        public DateTime? ReturnedDate { get; set; } //ngày trả sách (có thể trả sớm, muộn, hoặc đúng thời hạn)
        public BorrowingStatus? Status { get; set; }
        public Guid? BorrowingId { get; set; }
        public Borrowing? borrowing { get; set; }
        //public Guid? BookId { get; set; }
        //public Book? book { get; set; }
        public Guid? BookItemId { get; set; }
        public BookItem? bookItem { get; set; }
        public List<Fine>? fines { get; set; }






    }
}
