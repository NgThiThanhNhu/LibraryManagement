namespace DoAnCuoiKy.Model.Response.UserBehaviorResponse
{
    public class ModelStatus
    {
        public bool IsTrained { get; set; }
        public DateTime? LastTrainedAt { get; set; }
        public double? HoursSinceTraining { get; set; }
    }
}
