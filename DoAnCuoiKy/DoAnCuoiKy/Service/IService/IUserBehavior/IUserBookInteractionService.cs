using DoAnCuoiKy.Model.Request.UserBookInteractionRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.UserBehaviorRequest;

namespace DoAnCuoiKy.Service.IService.IUserBehavior
{
    public interface IUserBookInteractionService
    {
        Task<BaseResponse<UserBookInteractionResponse>> CreateRating(UserBookInteractionRequest userBookInteractionRequest);
        Task<BaseResponse<List<UserBookInteractionResponse>>> GetAllRating(Guid BookId);
    }
}
