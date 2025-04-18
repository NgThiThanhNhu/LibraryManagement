using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;

namespace DoAnCuoiKy.Service.IService.InformationLibrary.Kho
{
    public interface IRoomService
    {
        Task<BaseResponse<RoomResponse>> AddRoom(RoomRequest roomRequest);
        Task<BaseResponse<RoomResponse>> GetAllRoom();
        Task<BaseResponse<RoomResponse>> GetRoomById(Guid id);
        Task<BaseResponse<RoomResponse>> UpdateRoom(Guid id, RoomRequest roomRequest);
        Task<BaseResponse<RoomResponse>> DeleteRoom(Guid id);
    }
}
