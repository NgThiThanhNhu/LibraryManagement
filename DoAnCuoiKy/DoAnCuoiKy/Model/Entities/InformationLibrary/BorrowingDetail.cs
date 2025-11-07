using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class BorrowingDetail : BaseEntity
    {
        public Guid? Id { get; set; }
        public Guid? BorrowingId { get; set; }
        public Borrowing? borrowing { get; set; }
        public BookStatus? bookStatus { get; set; }
        public Guid? BookItemId { get; set; }
        public BookItem? bookItem { get; set; }
        public List<Fine>? fines { get; set; }
        //mối quan hệ giữa borrowing với lịch sử export
        public List<BookExportTransaction> bookExportTransactions { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public bool? IsFined { get; set; }=false;

    }
}
