namespace DoAnCuoiKy.Model.MachineLearning
{
    public class BookRecommendationResult
    {
        public Guid BookId { get; set; }
        public float PredictedScore { get; set; }
    }
}
