namespace DoAnCuoiKy.Model.MachineLearning
{
    public class RatingData
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public float Rating { get; set; }
    }
}
