using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;

namespace DoAnCuoiKy.Model.Request
{
    public class BorrowingRequest
    {
        public int Duration { get; set; }
        
        public List<Guid> BookiTemIds { get; set; } = new List<Guid>();
    }
}
