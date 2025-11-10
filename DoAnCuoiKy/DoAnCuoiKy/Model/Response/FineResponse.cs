using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Response
{
    public class FineResponse
    {
        public FineReason? FineReason { get; set; }
        public int? DaysLate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? FineRate { get; set; }
    }
}
