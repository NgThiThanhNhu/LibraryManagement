namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class BookChapter : BaseEntity
    {
        public Guid? Id { get; set; }
        public string? TitleChapter { get; set; }
        public List<Book>? books { get; set; }
        public List<BookItem>? bookItems { get; set; }
    }
}
