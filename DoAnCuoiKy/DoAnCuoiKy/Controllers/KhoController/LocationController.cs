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
    }
}
