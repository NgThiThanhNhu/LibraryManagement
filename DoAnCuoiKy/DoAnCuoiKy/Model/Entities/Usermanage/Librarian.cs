using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Entities.Notification;
using DoAnCuoiKy.Model.Entities.UserBehavior;
using System.Runtime.CompilerServices;

namespace DoAnCuoiKy.Model.Entities.Usermanage
{
    public class Librarian : BaseEntity
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public Guid? RoleId { get; set; } //phân quyền
        public Role? Role { get; set; }
        //1librarian-nhiều transaction
        public bool isValidate { get; set; } = false;
        public List<Borrowing> borrowings { get; set; }
        public List<Fine>? fines { get; set; }
        public List<BookCart> bookCarts { get; set; }
        public List<NotificationToUser> notifications { get; set; }
        public List<BookRecommendation> bookRecommendations { get; set; }
        public List<UserReadingSession> userReadingSessions { get; set; }
        public List<UserBookInteraction> userBookInteractions { get; set; }
        public List<UserCategoryPreference> categoryPreference { get; set; }
        public List<UserSearchHistory> userSearchHistories { get; set; }
    }
}
