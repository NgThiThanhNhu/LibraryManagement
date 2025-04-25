using DoAnCuoiKy.Model.Enum.InformationLibrary.Kho;

namespace DoAnCuoiKy.Model.Response.KhoResponse
{
    public class LocationResponse
    {
        public Guid? Id { get; set; }
        public string? SectionName { get; set; }
        public string? Description { get; set; }
        public string? RoomName { get; set; }
        public string? FloorName { get; set; }
        public string? ShelfName { get; set; }
        public string? BookShelfName {  get; set; }
        public LocationStatus? LocationStatus { get; set; }
    }
}
