using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService
{
    public interface IBorrowingDetailService
    {
        Task<BaseResponse<List<BorrowingDetailResponse>>> GetBorrowingDetailsForUser(Guid borrowingId);
        Task<BaseResponse<List<BorrowingDetailForFineResponse>>> GetBorrowingDetailsForFine (Guid borrowingId);
       
    }
}
