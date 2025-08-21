using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService
{
    public interface IBorrowingDetailService
    {
        Task<BaseResponse<List<BorrowingDetailResponse>>> GetBorrowingDetails(Guid borrowingId);
        Task<BaseResponse<List<BorrowingDetailResponse>>> GetBorrowingDetailsManaged ();
        Task<BaseResponse<BorrowingDetailResponse>> UpdateBorrowingDetail(Guid id, BorrowingDetailRequest borrowingDetailRequest);
    }
}
