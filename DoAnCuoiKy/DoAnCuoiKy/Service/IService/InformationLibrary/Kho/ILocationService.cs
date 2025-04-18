using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;

namespace DoAnCuoiKy.Service.IService.InformationLibrary.Kho
{
    public interface ILocationService
    {
        Task<BaseResponse<LocationResponse>> AddLocation(LocationRequest locationRequest);
        Task<BaseResponse<List<LocationResponse>>> GetAllLocation();
        Task<BaseResponse<LocationResponse>> GetLocationById(Guid id);
        Task<BaseResponse<LocationResponse>> UpdateLocation(Guid id, LocationRequest locationRequest);
        Task<BaseResponse<LocationResponse>> DeleteLocation(Guid id);

    }
}
