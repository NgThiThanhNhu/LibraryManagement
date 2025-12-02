using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class BookCart : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Librarian Librarian { get; set; }
        public DateTime ExpiredDate { get; set; }
        public CartStatus CartStatus { get; set; }
        public List<BookCartItem>? BookCartItems { get; set; }
    }
}
