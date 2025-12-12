using DoAnCuoiKy.Model.Request.UserBookInteractionRequest;
using DoAnCuoiKy.Model.Response.UserBehaviorResponse;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.IUserBehavior;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers.UserBehavior
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserReadingSessionController : ControllerBase
    {
        private readonly IUserReadingSessionService _userReadingSessionService;
        public UserReadingSessionController(IUserReadingSessionService userReadingSessionService)
        {
            _userReadingSessionService = userReadingSessionService;
        }
        [HttpPost("createReadingSession")]
        public async Task<BaseResponse<UserReadingSessionResponse>> CreateReadingSession(UserReadingSessionRequest readingSessionRequest)
        {
            BaseResponse<UserReadingSessionResponse> baseResponse = await _userReadingSessionService.CreateReadingSession(readingSessionRequest);
            return baseResponse;
        }
        [HttpPost("updateReadingSession/{sessionId}")]
        public async Task<BaseResponse<UserReadingSessionResponse>> UpdateReadingSession(Guid sessionId, UserReadingSessionUpdateRequest updateRequest)
        {
            BaseResponse<UserReadingSessionResponse> baseResponse = await _userReadingSessionService.UpdateReadingSession(sessionId, updateRequest);
            return baseResponse;
        }
        [HttpGet("book/{bookId}/last-position")]
        public async Task<BaseResponse<UserReadingSessionResponse>> GetLastPosition(Guid bookId)
        {
            BaseResponse<UserReadingSessionResponse> baseResponse = await _userReadingSessionService.GetLastPosition(bookId);
            return baseResponse;
        }
    }
}
