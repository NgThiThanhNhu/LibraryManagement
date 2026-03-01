namespace DoAnCuoiKy.Model.MachineLearningResponse
{
    public class TrainingResult
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<float> EpochErrors { get; set; } = new List<float>();
        public float FinalRMSE { get; set; }
    }
}
