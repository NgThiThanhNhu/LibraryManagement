using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class BookFile: BaseEntity
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Book book { get; set; }
        public string ImageUrl { get; set; }
        public string PublicIdImage { get; set; }  // publicId ảnh
        public string FileUrl { get; set; }
        public string PublicIdFile { get; set; }   // publicId file
        public BookFileType bookFileType { get; set; }
    }
}
