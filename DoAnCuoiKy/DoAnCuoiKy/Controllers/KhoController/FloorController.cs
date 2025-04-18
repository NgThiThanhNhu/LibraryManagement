using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Service.IService.InformationLibrary.Kho;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers.KhoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloorController : ControllerBase
    {
        private readonly IFloorService _floorService;
        public FloorController(IFloorService floorService)
        {
            _floorService = floorService;
        }

        [HttpPost("AddFloor")]
        public async Task<BaseResponse<FloorResponse>> addFloor(FloorRequest floorRequest)
        {
            BaseResponse<FloorResponse> response = await _floorService.AddFloor(floorRequest);
            return response;
        }
        [HttpGet("GetAllFloor")]
        public async Task<BaseResponse<List<FloorResponse>>> getAllFloor()
        {
            BaseResponse<List<FloorResponse>> response = await _floorService.GetAllFloor();
            return response;
        }
        [HttpGet("GetFloorById/{id}")]
        public async Task<BaseResponse<FloorResponse>> getFloorById(Guid id)
        {
            BaseResponse<FloorResponse> response = await _floorService.GetFloorById(id);
            return response;
        }
        [HttpPost("UpdateFloor/{id}")]
        public async Task<BaseResponse<FloorResponse>> updateFloor(Guid id, FloorRequest floorRequest)
        {
            BaseResponse<FloorResponse> response = await _floorService.UpdateFloor(id, floorRequest);
            return response;
        }
        [HttpPost("DeleteFloor/{id}")]
        public async Task<BaseResponse<FloorResponse>> deleteFloor(Guid id)
        {
            BaseResponse<FloorResponse> response = await _floorService.DeleteFloor(id);
            return response;
        }
    }
}
