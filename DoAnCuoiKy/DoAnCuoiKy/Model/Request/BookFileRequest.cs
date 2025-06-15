using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Request
{
    public class BookFileRequest
    {
        public Guid BookId { get; set; }
        public IFormFile UploadFile { get; set; }
        public IFormFile Image {  get; set; }
        public BookFileType BookFileType { get; set; }
    }
}
