using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.UserBehavior;

namespace DoAnCuoiKy.Model.Entities.UserBehavior
{
    public class UserBookInteraction : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Librarian Librarian { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public InteractionType InteractionType { get; set; }
        public DateTime InteractionDate { get; set; }
        public int Rating {  get; set; }
        public string ReviewText { get; set; }
        public int DurationSeconds { get; set; }
    }
}
