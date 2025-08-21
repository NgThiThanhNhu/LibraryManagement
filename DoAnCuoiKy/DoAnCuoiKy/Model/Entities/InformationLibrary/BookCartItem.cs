using DoAnCuoiKy.Model.Entities.Usermanage;

namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class BookCartItem : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Librarian User { get; set; }
        public Guid BookItemId { get; set; }
        public BookItem BookItem { get; set; }
    }
}
