namespace DoAnCuoiKy.Model.Entities.InformationLibrary.Kho
{
    public class ShelfSection : BaseEntity
    {
        public Guid? Id { get; set; }
        public string? SectionName { get; set; }
        public int Capacity { get; set; } //số lượng sách có thể chứa của 1 ô sách
      
        public Guid ShelfId { get; set; }
        public Shelf? Shelf { get; set; }
        //1 ô chứa nhiều bookitem mà location là vị trí của 1 bookitem trong ô suy ra 1 ô sẽ có nhiều location chi tiết của bookitem đó
        public List<BookItem>? BookItems { get; set; }  // Các sách nằm trong ô này
    }
}
