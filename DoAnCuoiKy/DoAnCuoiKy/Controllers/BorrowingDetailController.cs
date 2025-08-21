using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingDetailController : ControllerBase
    {
        private readonly IBorrowingDetailService _borrowingDetailService;
        public BorrowingDetailController(IBorrowingDetailService borrowingDetailService)
        {
            _borrowingDetailService = borrowingDetailService;
        }
        [HttpGet("getBorrowingDetails/{borrowingId}")]
        public async Task<BaseResponse<List<BorrowingDetailResponse>>> GetBorrowingDetails(Guid borrowingId)
        {
            BaseResponse<List<BorrowingDetailResponse>> baseResponse = await _borrowingDetailService.GetBorrowingDetails(borrowingId);
            return baseResponse;
        }
        [HttpGet("getBorrowingDetailsManaged")]
        public async Task<BaseResponse<List<BorrowingDetailResponse>>> GetBorrowingDetailsManaged()
        {
            BaseResponse<List<BorrowingDetailResponse>> baseResponse = await _borrowingDetailService.GetBorrowingDetailsManaged();
            return baseResponse;
        }
        [HttpPost("updateBorrowingDetail/{id}")]
        public async Task<BaseResponse<BorrowingDetailResponse>> UpdateBorrowingDetail(Guid id, BorrowingDetailRequest borrowingDetailRequest)
        {
            BaseResponse<BorrowingDetailResponse> response = await _borrowingDetailService.UpdateBorrowingDetail(id, borrowingDetailRequest);
            return response;
        }
    }
}
