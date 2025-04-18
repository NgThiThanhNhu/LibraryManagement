namespace DoAnCuoiKy.Model.Entities.InformationLibrary.Kho
{
    public class Location : LocationBase
    {
        public Guid? Id { get; set; }
        public Guid? ShelfSectionId {  get; set; }
        public ShelfSection? ShelfSection { get; set; }
        public List<BookItem>? BookItems { get; set; } //1 vị trí là địa chỉ củ 1 cuốn sách
        //nhưng 1 địa chỉ đó có chứa nhiều cuốn sách ví dụ: ở entity shelfsection có thuộc tính Capacity nghĩa là sức chứa của 1 ô section
        public string? Description { get; set; }

    }
}
