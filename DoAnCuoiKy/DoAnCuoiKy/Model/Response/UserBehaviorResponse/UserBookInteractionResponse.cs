using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.UserBehavior;

namespace DoAnCuoiKy.Model.Response.UserBehaviorRequest
{
    public class UserBookInteractionResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string BookTitle { get; set; }
        public DateTime CreateAt { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
    }
}
