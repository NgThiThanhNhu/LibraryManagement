using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Request
{
    public class BookItemRequest
    {
        
        public BookStatus? bookStatus { get; set; }
        //public Guid? BookCategoryId { get; set; }
        //public Guid? BookChapterId { get; set; }
        //public Guid? AuthorId { get; set; }
        //public Guid? PublisherId { get; set; }
        public Guid? BookId {  get; set; } 

    }
}
