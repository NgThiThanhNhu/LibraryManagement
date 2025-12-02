using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;

namespace DoAnCuoiKy.Model.Entities.UserBehavior
{
    public class UserCategoryPreference : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public decimal PreferenceScore { get; set; }
        public int ViewCount { get; set; }
        public int BorrowCount { get; set; }
        public int ReadingMinutes { get; set; }
        public DateTime LastInteraction {  get; set; }
        public Librarian Librarian { get; set; }
        public BookCategory BookCategory { get; set; }
    }
}
