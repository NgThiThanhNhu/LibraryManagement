using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Request
{
    public class FineRequest
    {
        public FineReason? fineReason { get; set; }
        public decimal FineRate { get; set; }
    }
}
