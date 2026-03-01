using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IDashboardService
    {
        Task<BaseResponse<DashboardResponse>> GetCurrentInformations();
        Task<BaseResponse<List<MostCommentBookResponse>>> GetMostCommentedBook();
        Task<BaseResponse<List<CategoryDistributionResponse>>> GetCategoryDistrubute();
    }
}
