namespace DoAnCuoiKy.Model.Request.KhoRequest
{
    public class ShelfRequest
    {
        public string? ShelfName { get; set; }
        public int? NumberOfSections { get; set; }
        public Guid BookshelfId { get; set; }  // Liên kết đến Tủ sách chứa Kệ sách này
    }
}
