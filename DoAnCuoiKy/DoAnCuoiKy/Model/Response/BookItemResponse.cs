using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class BookItemResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }   
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int? YearPublished { get; set; }
        public int? Quantity { get; set; }
        public BookStatus? bookStatus { get; set; }
        public string? CategoryName { get; set; }
        public string? TitleBookChapter { get; set; }
        public Guid? BookId { get; set; }

    }
}
