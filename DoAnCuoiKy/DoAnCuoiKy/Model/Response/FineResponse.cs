using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class FineResponse
    {
        public FineReason? FineReason { get; set; }
        public int? DaysLate { get; set; }
        public float? Amount { get; set; }
        public float? FineRate { get; set; }
    }
}
