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
        [HttpGet("borrowings/{borrowingId}/details/user")]
        public async Task<BaseResponse<List<BorrowingDetailResponse>>> GetBorrowingDetailsForUser(Guid borrowingId)
        {
            BaseResponse<List<BorrowingDetailResponse>> baseResponse = await _borrowingDetailService.GetBorrowingDetailsForUser(borrowingId);
            return baseResponse;
        }
        [HttpGet("borrowings/{borrowingId}/details/fine")]
        public async Task<BaseResponse<List<BorrowingDetailForFineResponse>>> GetBorrowingDetailsForFine(Guid borrowingId)
        {
            BaseResponse<List<BorrowingDetailForFineResponse>> baseResponse = await _borrowingDetailService.GetBorrowingDetailsForFine(borrowingId);
            return baseResponse;
        }
        
    }
}
