namespace DoAnCuoiKy.Model.Enum.InformationLibrary
{
    public enum BookStatus : byte
    {
        Available = 0, //có sẵn
        Borrowed = 2, //đã mượn
        Lost = 4, //mất sách 
        Damaged = 5, // hỏng sách
    }
}
