namespace DoAnCuoiKy.Model.Entities.InformationLibrary.Kho
{
    public class Shelf : BaseEntity
    {
        public Guid? Id { get; set; }
        public string? ShelfName { get; set; }
        public int? NumberOfSections { get; set; }
        public Guid? BookshelfId { get; set; }  // Liên kết đến Tủ sách chứa Kệ sách này
        public BookShelf? Bookshelf { get; set; }  // Mối quan hệ với Tủ sách
        public List<ShelfSection>? Sections { get; set; }
    }
}
