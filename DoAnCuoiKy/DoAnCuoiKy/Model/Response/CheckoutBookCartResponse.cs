namespace DoAnCuoiKy.Model.Response
{
    public class CheckoutBookCartResponse
    {
        public BorrowingResponse BorrowingResponse { get; set; }
        public int TotalBorrowedItems { get; set; }
    }
}
