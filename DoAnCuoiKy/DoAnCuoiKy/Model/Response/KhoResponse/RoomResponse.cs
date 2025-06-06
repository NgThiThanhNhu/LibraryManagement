namespace DoAnCuoiKy.Model.Response.KhoResponse
{
    public class RoomResponse
    {
        public Guid? Id { get; set; }
        public string? RoomName { get; set; }
        public int? MaxBookShelfCapity { get; set; }


        // Tính số tủ sách hiện tại dựa trên BookShelves
        public int CurrentBookShelves { get; set; }

        // Tự động xác định phòng có đầy hay chưa
        public bool IsFull => CurrentBookShelves >= MaxBookShelfCapity;

        public Guid? FloorId { get; set; }
        public string? FloorName { get; set; }
    }
}
