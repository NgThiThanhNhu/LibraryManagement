using DoAnCuoiKy.Model.Enum.InformationLibrary.Kho;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary.Kho
{
    public class Location : BaseEntity
    {
        public Guid? Id { get; set; }
        public Guid? ShelfSectionId {  get; set; }
        public ShelfSection? ShelfSection { get; set; }
        public List<BookItem>? BookItems { get; set; } //1 vị trí là địa chỉ củ 1 cuốn sách
        //nhưng 1 địa chỉ đó có chứa nhiều cuốn sách ví dụ: ở entity shelfsection có thuộc tính Capacity nghĩa là sức chứa của 1 ô section
        public string? Description { get; set; }
        //khi thêm bookitem thì location của kho phải có trước, và biết tình trạng của location đó còn trống hay không
        //khi thêm bookitem thì chọn vị trí để sách vào 
        public LocationStatus? LocationStatus { get; set; } = 0;


    }
}
