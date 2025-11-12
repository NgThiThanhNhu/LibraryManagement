using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService
{
    public interface IFineService
    {
        Task<BaseResponse<List<FineResponse>>> CreateFine(Guid borrowingDetailId, List<FineRequest> fineRequests);
        Task<BaseResponse<FineResponse>> GetFineById(Guid borrowingDetailId);
    }
}
