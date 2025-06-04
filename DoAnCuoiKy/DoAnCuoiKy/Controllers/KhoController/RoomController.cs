using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Service.IService.InformationLibrary.Kho;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers.KhoController
{

    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }
        [HttpPost("AddRoom")]
        public async Task<BaseResponse<RoomResponse>> AddRoom(RoomRequest roomRequest)
        {
           BaseResponse<RoomResponse> response = await _roomService.AddRoom(roomRequest);
            return response;
        }
        [HttpPost("DeleteRoom/{id}")]
        public async Task<BaseResponse<RoomResponse>> DeleteRoom(Guid id)
        {
            BaseResponse<RoomResponse> response = await _roomService.DeleteRoom(id);
            return response;
        }
        [HttpGet("GetAllRoom")]
        public async Task<BaseResponse<List<RoomResponse>>> GetAllRoom()
        {
            BaseResponse<List<RoomResponse>> response = await _roomService.GetAllRoom();
            return response;
        }
        [HttpGet("GetRoomById/{id}")]
        public async Task<BaseResponse<RoomResponse>> GetRoomById(Guid id)
        {
            BaseResponse<RoomResponse> response = await _roomService.GetRoomById(id);
            return response;
        }
        [HttpPost("UpdateRoom/{id}")]
        public async Task<BaseResponse<RoomResponse>> UpdateRoom(Guid id, RoomRequest roomRequest)
        {
            BaseResponse<RoomResponse> response = await _roomService.UpdateRoom(id, roomRequest);
            return response;
        }
    }
}
