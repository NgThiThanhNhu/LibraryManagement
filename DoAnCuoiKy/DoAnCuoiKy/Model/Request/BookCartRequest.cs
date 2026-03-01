namespace DoAnCuoiKy.Model.Request
{
    public class BookCartRequest
    {
        public Guid BookId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
