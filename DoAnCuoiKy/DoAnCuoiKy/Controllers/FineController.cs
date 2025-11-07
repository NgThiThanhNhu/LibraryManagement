using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FineController : ControllerBase
    {
        private readonly IFineService _fineService;
        public FineController(IFineService fineService)
        {
            _fineService = fineService;
        }
        [HttpPost("createFine")]
        public async Task<BaseResponse<List<FineResponse>>> CreateFine(Guid borrowingDetailId, List<FineRequest> fineRequests)
        {
            BaseResponse<List<FineResponse>> baseResponse = await _fineService.CreateFine(borrowingDetailId, fineRequests);
            return baseResponse;
        }
    }
}
