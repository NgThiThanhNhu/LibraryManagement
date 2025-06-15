namespace DoAnCuoiKy.Model.Response.KhoResponse
{
    public class BookShelfResponse
    {
        public Guid? Id { get; set; }
        public string? BookShelfName { get; set; }
        public int? NumberOfShelves { get; set; } // số kệ trong 1 tủ
        public Guid? RoomId { get; set; }
        public string? RoomName { get; set; }
       
        public int CurrentShelves { get; set; }

        
        public bool IsFull => CurrentShelves >= NumberOfShelves;
    }
}
