namespace DoAnCuoiKy.Model.Response.KhoResponse
{
    public class ShelfResponse
    {
        public Guid? Id { get; set; }
        public string? ShelfName { get; set; }
        public int? NumberOfSections { get; set; } // tổng số ô kệ có
        public Guid BookshelfId { get; set; }
        public string? BookshelfName { get; set; } 
        public int CurrentSection { get; set; } // số ô hiện tại đang có của kệ
        public bool isFull => CurrentSection >= NumberOfSections;
        
    }
}
