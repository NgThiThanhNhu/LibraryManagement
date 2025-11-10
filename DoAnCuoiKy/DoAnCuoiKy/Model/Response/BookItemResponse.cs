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
        
        public decimal? UnitPrice { get; set; }
        public BookStatus? BookStatus { get; set; }
        public string? CategoryName { get; set; }
        public string? TitleBookChapter { get; set; }
       
       
        public string ShelfSectionName { get; set; }
        public List<string> imageUrl { get; set; } = new List<string>();
       

    }
}
