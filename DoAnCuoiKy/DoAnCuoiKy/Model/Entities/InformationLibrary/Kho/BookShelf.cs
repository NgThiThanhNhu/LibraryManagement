namespace DoAnCuoiKy.Model.Entities.InformationLibrary.Kho
{
    public class BookShelf : BaseEntity
    {
        public Guid Id { get; set; }
        public string? BookShelfName { get; set; }
        public int? NumberOfShelves { get; set; } // số kệ trong 1 tủ
        public List<Shelf>? Shelves { get; set; }
        public Guid? RoomId { get; set; }
        public Room? Room { get; set; }
    }
}
