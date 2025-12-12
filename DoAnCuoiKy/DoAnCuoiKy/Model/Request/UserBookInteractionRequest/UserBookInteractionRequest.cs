using DoAnCuoiKy.Model.Enum.UserBehavior;

namespace DoAnCuoiKy.Model.Request.UserBookInteractionRequest
{
    public class UserBookInteractionRequest
    {
        public Guid BookId { get; set; }
        public InteractionType InteractionType { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
    }
}
