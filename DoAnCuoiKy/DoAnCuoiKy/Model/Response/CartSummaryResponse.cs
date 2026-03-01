namespace DoAnCuoiKy.Model.Response
{
    public class CartSummaryResponse
    {
        public List<BookCartItemResponse> Items { get; set; }
        public int TotalBooks { get; set; }          // Số loại sách
        public int TotalQuantity { get; set; }       // Tổng số quyển
        public int RemainingSlots { get; set; }      // Còn có thể thêm
        public bool CanAddMore { get; set; }
    }
}
