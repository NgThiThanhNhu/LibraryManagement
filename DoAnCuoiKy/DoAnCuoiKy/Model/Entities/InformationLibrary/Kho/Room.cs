namespace DoAnCuoiKy.Model.Entities.InformationLibrary.Kho
{
    public class Room : BaseEntity
    {
        public Guid? Id { get; set; }
        public string? RoomName { get; set; }
        public int? MaxBookShelfCapity { get; set; }
        public Guid? FloorId { get; set; }
        public Floor? Floor { get;set; }
        public List<BookShelf>? BookShelves { get; set; }
    }
}
