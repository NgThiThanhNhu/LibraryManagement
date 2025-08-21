using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Model
{
    public class OtpData
    {
        public string Code { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
