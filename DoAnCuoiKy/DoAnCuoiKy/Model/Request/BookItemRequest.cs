using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Request
{
    public class BookItemRequest
    {
        
        public BookStatus? bookStatus { get; set; }
      //coi ở đây để xử lý logic dữ liệu bookitem và nơi lưu trữ nó, phân bổ
        public Guid? BookId {  get; set; } 

    }
}
