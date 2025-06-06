namespace DoAnCuoiKy.Model.Response.KhoResponse
{
    public class ShelfResponse
    {
        public Guid? Id { get; set; }
        public string? ShelfName { get; set; }
        public int? NumberOfSections { get; set; }
        public Guid? BookshelfId { get; set; }
        public string? BookshelfName { get; set; }  // Liên kết đến Tủ sách chứa Kệ sách này
    }
}
