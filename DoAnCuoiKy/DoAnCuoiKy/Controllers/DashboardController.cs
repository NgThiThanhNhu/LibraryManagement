using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet("system-information")]
        public async Task<BaseResponse<DashboardResponse>> GetCurrentInformations()
        {
            BaseResponse<DashboardResponse> baseResponse = await _dashboardService.GetCurrentInformations();
            return baseResponse;
        }
        [HttpGet("most-commented-books")]
        public async Task<BaseResponse<List<MostCommentBookResponse>>> GetMostCommentedBook()
        {
            BaseResponse<List<MostCommentBookResponse>> baseResponse = await _dashboardService.GetMostCommentedBook();
            return baseResponse;
        }
        [HttpGet("category-distribution")]
        public async Task<BaseResponse<List<CategoryDistributionResponse>>> GetCategoryDistrubute()
        {
            BaseResponse<List<CategoryDistributionResponse>> baseResponse = await _dashboardService.GetCategoryDistrubute();
            return baseResponse;
        }
    }
}
