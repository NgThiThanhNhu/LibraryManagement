namespace DoAnCuoiKy.Model.Entities.InformationLibrary.Kho
{
    public class Floor : BaseEntity
    {
        public Guid? Id { get; set; }
        public string? FloorName { get; set; }
        public List<Room>? Rooms { get; set; }
    }
}
