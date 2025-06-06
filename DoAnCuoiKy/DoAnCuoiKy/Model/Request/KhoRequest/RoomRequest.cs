namespace DoAnCuoiKy.Model.Request.KhoRequest
{
    public class RoomRequest
    {
        public string? RoomName { get; set; }
        public int? MaxBookShelfCapity { get; set; }
       
        public Guid? FloorId { get; set; }
    }
}
