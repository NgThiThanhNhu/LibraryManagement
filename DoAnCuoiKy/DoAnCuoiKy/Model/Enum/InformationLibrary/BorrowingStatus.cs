namespace DoAnCuoiKy.Model.Enum.InformationLibrary
{
    public enum BorrowingStatus : byte
    {
        Wait = 0, // chờ duyệt
        Approved = 1,
        Scheduled = 2,
        Borrowing = 3, //đang mượn
        Returned = 4, //đã trả cho từng cuốn sách borrowingdetail, sau đó mới tổng lại đã trã cho từng phiếu mượn, lúc đó mới cập nhật tình trạng sách bookitem lại là avai và tăng số lượng sách lên lại, borrowingdetail liên kết với fine
        Overdue = 5, //quá hạn
        Reject = 6 // từ chối
    }
}
