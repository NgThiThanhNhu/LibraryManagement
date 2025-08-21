namespace DoAnCuoiKy.Model.Enum.InformationLibrary
{
    public enum BookStatus : byte
    {
        Available = 0, //có sẵn
        Borrowed = 1, //đã mượn
        Reserved = 2, //đã được đặt trước
        Lost = 3, //mất sách
        Damaged = 4, // hỏng sách
    }
}
