namespace DoAnCuoiKy.Model.Enum.InformationLibrary
{
    public enum BookReservationStatus : byte
    {
        WaitConfirm = 0,//đợi xác nhận
        Confirmed = 1, //đã được xác nhận
        Cancel = 2, //người dùng hủy đặt trước
        Expire = 3 //đến ngày lấy sách nhưng không đến mượn sách
    }
}
