using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.UserBehavior;

namespace DoAnCuoiKy.Model.Entities.UserBehavior
{
    public class BookRecommendation : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Librarian Librarian { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public decimal Score { get; set; }
        public string Reason { get; set; }
        public RecommendationType RecommendationType { get; set; }
        public bool IsClicked { get; set; }

    }
}
