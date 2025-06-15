namespace DoAnCuoiKy.Model.Response.KhoResponse
{
    public class ShelfSectionResponse
    {
        public Guid? Id { get; set; }
        public string? SectionName { get; set; }
        public int? Capacity { get; set; } //số lượng sách có thể chứa của 1 ô sách
       
        public Guid ShelfId { get; set; }
        public string? ShelfName { get; set; }
        public int CurrentBookItem { get; set; }
        public bool isFull => CurrentBookItem >= Capacity;
    }
}
