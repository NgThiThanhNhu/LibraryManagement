namespace DoAnCuoiKy.Model.Response
{
    public class BookDetailResponse
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? AuthorName { get; set; }
        public string? PublisherName { get; set; }
        public int? YearPublished { get; set; }
        public int? Quantity { get; set; } //số lượng sách nhập vào
        public decimal? UnitPrice { get; set; }
        public string? CategoryName { get; set; }
        public string? TitleBookChapter { get; set; }
        public string Description { get; set; }
        public List<Guid> BookFileId { get; set; } = new List<Guid>();
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}
