using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.UserBehavior;

namespace DoAnCuoiKy.Model.Response.UserBehaviorResponse
{
    public class RecommendationResponse
    {
        public Guid RecommendationId { get; set; }  // Để track clicks
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public string CoverImageUrl { get; set; }
        public float PredictedScore { get; set; }
        public int Rank { get; set; }
        public string Reason { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
