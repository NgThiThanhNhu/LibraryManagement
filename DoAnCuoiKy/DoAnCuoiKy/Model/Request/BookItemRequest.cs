using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Request
{
    public class BookItemRequest
    {
        
        public BookStatus? bookStatus { get; set; }
      
        public Guid? BookId {  get; set; } 

    }
}
