using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService
{
    public interface IBorrowingService
    {
        Task<BaseResponse<BorrowingResponse>> CreateBorrowing(BorrowingRequest borrowingRequest);
        Task<BaseResponse<ReplyBorrowingResponse>> UpdateBorrowing(Guid id, ReplyBorrowingRequest replyBorrowingRequest);
        Task<BaseResponse<List<BorrowingUserResponse>>> GetAllUserBorrowing();
        Task<BaseResponse<List<BorrowingResponse>>> GetAllAdminBorrowing();
        
    }
}
