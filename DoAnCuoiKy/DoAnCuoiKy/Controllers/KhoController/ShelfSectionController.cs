using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Service.IService.InformationLibrary.Kho;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers.KhoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShelfSectionController : ControllerBase
    {
        private readonly IShelfSectionService _shelfSectionService;
        public ShelfSectionController(IShelfSectionService shelfSectionService)
        {
            _shelfSectionService = shelfSectionService;
        }

        [HttpPost("AddShelfSection")]   
        public async Task<BaseResponse<ShelfSectionResponse>> addShelfSection(ShelfSectionRequest shelfSectionRequest)
        {
            BaseResponse<ShelfSectionResponse> response = await _shelfSectionService.AddShelfSection(shelfSectionRequest);
            return response;
        }

        [HttpGet("GetAllShelfSection")]
        public async Task<BaseResponse<List<ShelfSectionResponse>>> getAllShelfSection()
        {
            BaseResponse<List<ShelfSectionResponse>> response = await _shelfSectionService.GetAllShelfSection();
            return response;
        }

        [HttpGet("GetShelfSectionById/{id}")]
        public async Task<BaseResponse<ShelfSectionResponse>> getShelfSectionById(Guid id)
        {
            BaseResponse<ShelfSectionResponse> response = await _shelfSectionService.GetShelfSectionById(id);
            return response;
        }

        [HttpPost("UpdateShelfSection/{id}")]
        public async Task<BaseResponse<ShelfSectionResponse>> updateShelfSection(Guid id, ShelfSectionRequest shelfSectionRequest)
        {
            BaseResponse<ShelfSectionResponse> response = await _shelfSectionService.UpdateShelfSection(id, shelfSectionRequest);
            return response;
        }

        [HttpPost("DeleteShelfSection/{id}")]
        public async Task<BaseResponse<ShelfSectionResponse>> deleteShelfSection(Guid id)
        {
            BaseResponse<ShelfSectionResponse> response = await _shelfSectionService.DeleteShelfSection(id);
            return response;
        }
    }
}
