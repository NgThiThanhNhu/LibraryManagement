using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Request
{
    public class BookRequest
    {
        public string? Title { get; set; }
        public Guid? AuthorId { get; set; }
        public Guid? PublisherId { get; set; }
        public int? YearPublished { get; set; }
        public int? Quantity { get; set; } //số lượng sách nhập vào
        public Guid? CategoryId { get; set; }
        public Guid? BookChapterId { get; set; }
        public float? TotalPrice { get; set; }
        public float? UnitPrice { get; set; }
    }
}
