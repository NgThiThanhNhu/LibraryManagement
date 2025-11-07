using DoAnCuoiKy.Model.Entities.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class BookCartItemResponse
    {
        public Guid Id { get; set; }
        public Guid BookItemId { get; set; }
        public string BookItemTitle { get; set; }
        public string BookItemAuthor { get; set; }
        public string BookItemCategory { get; set; }
        public Guid UserId { get; set; }
        public string UserName  { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
