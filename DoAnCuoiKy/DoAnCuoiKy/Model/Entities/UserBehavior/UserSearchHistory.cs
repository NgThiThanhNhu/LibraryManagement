using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;

namespace DoAnCuoiKy.Model.Entities.UserBehavior
{
    public class UserSearchHistory : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string SearchKeyword { get; set; }
        public string SearchType { get; set; }
        public int ResultCount { get; set; }
        public List<Book> Books { get; set; }
        public string IpAddress { get; set; }
        public Librarian Librarian { get; set; }
    }
}
