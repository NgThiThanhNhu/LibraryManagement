using DoAnCuoiKy.Model.Request.UserBookInteractionRequest;
using DoAnCuoiKy.Model.Response.UserBehaviorRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.IUserBehavior;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers.UserBehavior
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBookInteractionController : ControllerBase
    {
        private readonly IUserBookInteractionService _userBookInteractionService;
        public UserBookInteractionController(IUserBookInteractionService userBookInteractionService)
        {
            _userBookInteractionService = userBookInteractionService;
        }
        [HttpPost("createComment")]
        public async Task<BaseResponse<UserBookInteractionResponse>> CreateRating(UserBookInteractionRequest userBookInteractionRequest)
        {
            BaseResponse<UserBookInteractionResponse> baseResponse = await _userBookInteractionService.CreateRating(userBookInteractionRequest);
            return baseResponse;
        }
        [HttpGet("getListComment/{BookId}")]
        public async Task<BaseResponse<List<UserBookInteractionResponse>>> GetAllRating(Guid BookId)
        {
            BaseResponse<List<UserBookInteractionResponse>> baseResponse = await _userBookInteractionService.GetAllRating(BookId);
            return baseResponse;
        }
    }
}
