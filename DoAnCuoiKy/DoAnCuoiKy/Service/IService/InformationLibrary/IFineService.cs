using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService
{
    public interface IFineService
    {
        Task<BaseResponse<FineResponse>> CreateFine(Guid BorrowingDetailId, FineReason fineReason);
    }
}
