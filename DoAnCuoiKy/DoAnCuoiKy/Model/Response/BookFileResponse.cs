using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class BookFileResponse
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string BookName { get; set; }
        public string FileUrl { get; set; }
        public string ImageUrl { get; set; }
        public string PublicIdImage { get; set; }
        public string PublicIdFile { get; set; }

        public BookFileType Type { get; set; }
        
    }
}
