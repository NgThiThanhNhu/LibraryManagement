namespace DoAnCuoiKy.Model.Response
{
    public class BookCartItemResponse
    {
        public Guid BookCartItemId { get; set; }
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string ImageUrl { get; set; }
        public int RequestedQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public bool CanIncrease { get; set; }
        public bool CanDecrease { get; set; }
        public string StatusText { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
