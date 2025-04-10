using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class BookResponse 
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? AuthorName { get; set; }
        public string? PublisherName { get; set; }
        public int? YearPublished { get; set; }
        public int? Quantity { get; set; } //số lượng sách nhập vào
        public float? TotalPrice { get; set; }
        public float? UnitPrice { get; set; }
        public string? CategoryName { get; set; }
        public string? TitleBookChapter { get; set; }

    }
}
