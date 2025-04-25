using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary.Kho;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers.KhoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        [HttpPost("AddLocation")]
        public async Task<BaseResponse<LocationResponse>> addLocation(LocationRequest locationRequest)
        {
            BaseResponse<LocationResponse> response = await _locationService.AddLocation(locationRequest);
            return response;
        }
        [HttpGet("GetAllLocation")]
        public async Task<BaseResponse<List<LocationResponse>>> getAllLocation()
        {
            BaseResponse<List<LocationResponse>> response = await _locationService.GetAllLocation();
            return response;
        }
        [HttpGet("GetLocationById/{id}")]
        public async Task<BaseResponse<LocationResponse>> getLocationById(Guid id)
        {
            BaseResponse<LocationResponse> response = await _locationService.GetLocationById(id);
            return response;
        }
        [HttpPost("UpdateLocation/{id}")]
        public async Task<BaseResponse<LocationResponse>> UpdateLocation(Guid id, LocationRequest locationRequest)
        {
            BaseResponse<LocationResponse> response = await _locationService.UpdateLocation(id, locationRequest);
            return response;
        }
        [HttpPost("DeleteLocation/{id}")]
        public async Task<BaseResponse<LocationResponse>> DeleteLocation(Guid id)
        {
            BaseResponse<LocationResponse> response = await _locationService.DeleteLocation(id);
            return response;
        }
    }
}
