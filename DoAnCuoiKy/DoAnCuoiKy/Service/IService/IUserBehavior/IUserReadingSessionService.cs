using DoAnCuoiKy.Model.Request.UserBookInteractionRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.UserBehaviorResponse;

namespace DoAnCuoiKy.Service.IService.IUserBehavior
{
    public interface IUserReadingSessionService
    {
        Task<BaseResponse<UserReadingSessionResponse>> CreateReadingSession(UserReadingSessionRequest readingSessionRequest);
        Task<BaseResponse<UserReadingSessionResponse>> UpdateReadingSession(Guid sessionId, UserReadingSessionUpdateRequest updateRequest);
        Task<BaseResponse<UserReadingSessionResponse>> GetLastPosition(Guid bookId);
    }
}
