using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class BookItemResponse
    {
        public Guid? Id { get; set; }
        public string BarCode { get; set; }
        public string? Title { get; set; }   
        public string? AuthorName { get; set; }
        public string? PublisherName { get; set; }
        public int? YearPublished { get; set; }
        public int? Quantity { get; set; }
        public float? UnitPrice { get; set; }
        public BookStatus? BookStatus { get; set; }
        public string? CategoryName { get; set; }
        public string? TitleBookChapter { get; set; }
        public Guid? BookId { get; set; }
        public Guid? ShelfSectionId { get; set; }
        public string ShelfSectionName { get; set; }

    }
}
