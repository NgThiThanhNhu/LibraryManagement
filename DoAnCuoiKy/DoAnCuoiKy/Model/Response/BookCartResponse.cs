using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class BookCartResponse
    {
        public Guid BookCartId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreateAt { get; set; }
        public int TotalBooks { get; set; }        // Số loại sách
        public int TotalQuantity { get; set; }     // Tổng số quyển
        public int RemainingSlots { get; set; }    // Còn có thể thêm
        public bool CanAddMore { get; set; }
        public DateTime ExpiredDate { get; set; }
        public CartStatus CartStatus { get; set; }
        public DateTime UpdateAt { get; set; }
        public List<BookCartItemResponse> bookCartItemResponses { get; set; }
    }
}
