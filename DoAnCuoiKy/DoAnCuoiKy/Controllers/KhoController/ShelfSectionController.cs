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
    }
}
