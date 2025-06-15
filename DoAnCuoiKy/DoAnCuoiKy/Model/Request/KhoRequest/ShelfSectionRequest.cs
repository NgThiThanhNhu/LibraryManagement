namespace DoAnCuoiKy.Model.Request.KhoRequest
{
    public class ShelfSectionRequest
    {
        public string? SectionName { get; set; }
        public int Capacity { get; set; } //số lượng sách có thể chứa của 1 ô sách
        public Guid ShelfId { get; set; }
    }
}
