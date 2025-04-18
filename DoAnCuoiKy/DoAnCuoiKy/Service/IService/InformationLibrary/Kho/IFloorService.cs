using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;

namespace DoAnCuoiKy.Service.IService.InformationLibrary.Kho
{
    public interface IFloorService
    {
        Task<BaseResponse<FloorResponse>> AddFloor(FloorRequest floorRequest);
        Task<BaseResponse<List<FloorResponse>>> GetAllFloor();
        Task<BaseResponse<FloorResponse>> GetFloorById(Guid id);
        Task<BaseResponse<FloorResponse>> UpdateFloor(Guid id, FloorRequest floorRequest);
        Task<BaseResponse<FloorResponse>> DeleteFloor(Guid id);
    }
}
