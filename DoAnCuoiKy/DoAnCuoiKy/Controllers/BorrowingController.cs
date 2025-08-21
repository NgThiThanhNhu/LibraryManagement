using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingController : ControllerBase
    {
        private readonly IBorrowingService _borrowingService;
        public BorrowingController(IBorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }
        [HttpPost("createBorrowing")]
        public async Task<BaseResponse<BorrowingResponse>> createBorrowing(BorrowingRequest borrowingRequest)
        {
            BaseResponse<BorrowingResponse> baseResponse = await _borrowingService.CreateBorrowing(borrowingRequest);
            return baseResponse;
        }
        [HttpPost("updateBorrowing/{id}")]
        public async Task<BaseResponse<ReplyBorrowingResponse>> updateBorrowing(Guid id, ReplyBorrowingRequest replyBorrowingRequest)
        {
            BaseResponse<ReplyBorrowingResponse> baseResponse = await _borrowingService.UpdateBorrowing(id, replyBorrowingRequest);
            return baseResponse;
        }
        [HttpGet("getAllUserBorrowing")]
        public async Task<BaseResponse<List<BorrowingUserResponse>>> GetAllUserBorrowing()
        {
            BaseResponse<List<BorrowingUserResponse>> baseResponse = await _borrowingService.GetAllUserBorrowing();
            return baseResponse;
        }
        [HttpGet("getAllBorrowing")]
        public async Task<BaseResponse<List<BorrowingResponse>>> GetAllAdminBorrowing()
        {
            BaseResponse<List<BorrowingResponse>> baseResponse = await _borrowingService.GetAllAdminBorrowing();
            return baseResponse;
        }
    }
}
