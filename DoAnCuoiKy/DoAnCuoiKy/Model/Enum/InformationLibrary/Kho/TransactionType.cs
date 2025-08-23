namespace DoAnCuoiKy.Model.Enum.InformationLibrary.Kho
{
    public enum TransactionType : byte
    {
        Import = 0,      // Nhập sách mới
        Export = 1,      // Xuất sách (giao cho mượn)
        ReturnToStock = 2, // Trả sách về kho dành cho borrowingstatus return
        Remove = 3       // Loại bỏ sách khỏi kho (sách hư, mất, không còn lưu hành)
    }
}
