using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Entities.UserBehavior;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using System.ComponentModel.DataAnnotations;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class Book : BaseEntity
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public Guid? PublisherId { get; set; }
        public Publisher? Publisher { get; set; }
        public Guid? BookAuthorId { get; set; }
        public BookAuthor BookAuthor { get; set; }
        public int? YearPublished { get; set; }
        public int Quantity { get; set; } //số lượng sách nhập
        public decimal? TotalPrice { get; set; }
        public decimal? UnitPrice { get; set; }
        public Guid? CategoryId { get; set; }
        public BookCategory? Category { get; set; }
        public Guid? BookChapterId { get; set; }
        public BookChapter? BookChapter { get; set; }
        public List<BookItem>? bookItems { get; set; }
        public List<BookImportTransaction> ImportTransactions { get; set; }
        public List<BookFile> bookFiles { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public List<BookCartItem> BookCartItems { get; set; }
        public List<BookRecommendation> bookRecommendations { get; set; }
        public List<UserBookInteraction> userBookInteractions { get; set; }
        public List<UserReadingSession> userReadingSessions { get; set; }
    }
}
