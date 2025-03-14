using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Request
{
    public class BookItemRequest
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public int? YearPublished { get; set; }
        public int? Quantity { get; set; }
        public BookStatus? bookStatus { get; set; }
        public Guid? BookCategoryId { get; set; }
        public Guid? BookChapterId { get; set; }
        public Guid? BookId {  get; set; } 

    }
}
