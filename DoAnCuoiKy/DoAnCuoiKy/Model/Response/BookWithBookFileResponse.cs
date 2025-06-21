using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class BookWithBookFileResponse
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }     // từ BookFile
        //public string PdfUrl { get; set; }       // từ BookFile
        //public BookFileType BookFileType { get; set; }
    }
}
