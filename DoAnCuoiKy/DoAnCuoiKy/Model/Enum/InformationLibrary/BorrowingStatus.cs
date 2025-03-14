namespace DoAnCuoiKy.Model.Enum.InformationLibrary
{
    public enum BorrowingStatus : byte
    {
        Borrowed = 0, //đang mượn
        Returned = 1, //đã trả
        Overdue = 2 //quá hạn
    }
}
