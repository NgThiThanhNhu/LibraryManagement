using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Service.IService.InformationLibrary.Kho;

namespace DoAnCuoiKy.Service.InformationLibrary.Kho
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;
        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<BaseResponse<LocationResponse>> AddLocation(LocationRequest locationRequest)
        {
            BaseResponse<LocationResponse> response = new BaseResponse<LocationResponse>();
            Location location = new Location();
            location.Room = locationRequest.room
        }

        public Task<BaseResponse<LocationResponse>> DeleteLocation(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<List<LocationResponse>>> GetAllLocation()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<LocationResponse>> GetLocationById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<LocationResponse>> UpdateLocation(Guid id, LocationRequest locationRequest)
        {
            throw new NotImplementedException();
        }
    }
}
