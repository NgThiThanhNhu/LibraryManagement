using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary.Kho;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers.KhoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShelfController : ControllerBase
    {
        private readonly IShelfService _shelfService;
        public ShelfController(IShelfService shelfService)
        {
            _shelfService = shelfService;
        }
        [HttpPost("AddShelf")]
        public async Task<BaseResponse<ShelfResponse>> addShelf(ShelfRequest shelfRequest)
        {
            BaseResponse<ShelfResponse> response = await _shelfService.AddShelf(shelfRequest);
            return response;
        }
        [HttpGet("GetAllShelf")]
        public async Task<BaseResponse<List<ShelfResponse>>> getAllShelf()
        {
            BaseResponse<List<ShelfResponse>> response = await _shelfService.GetAllShelf();
            return response;
        }
        [HttpGet("GetShelfById/{id}")]
        public async Task<BaseResponse<ShelfResponse>> getShelfById(Guid id)
        {
            BaseResponse<ShelfResponse> response = await _shelfService.GetShelfById(id);
            return response;
        }
        [HttpPost("UpdateShelf/{id}")]
        public async Task<BaseResponse<ShelfResponse>> updateShelf(Guid id, ShelfRequest shelfRequest)
        {
            BaseResponse<ShelfResponse> response = await _shelfService.UpdateShelf(id, shelfRequest);
            return response;
        }
        [HttpPost("DeleteShelf/{id}")]
        public async Task<BaseResponse<ShelfResponse>> deleteShelf(Guid id)
        {
            BaseResponse<ShelfResponse> response = await _shelfService.DeleteShelf(id);
            return response;
        }
    }
}
