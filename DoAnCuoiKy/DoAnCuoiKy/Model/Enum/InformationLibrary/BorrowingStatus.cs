namespace DoAnCuoiKy.Model.Enum.InformationLibrary
{
    public enum BorrowingStatus : byte
    {
        Wait = 0, // chờ duyệt
        Approved = 1,
        Scheduled = 2,
        Borrowing = 3, //đang mượn
        Returned = 4, //đã trả
        Overdue = 5, //quá hạn
        Reject = 6 // từ chối
    }
}
