using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Enum.InformationLibrary.Kho;

namespace DoAnCuoiKy.Model.Request.KhoRequest
{
    public class LocationRequest
    {
        public Guid? ShelfSectionId { get; set; }
        public string? Description { get; set; }
        public LocationStatus? LocationStatus { get; set; };

    }
}
