using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoAnCuoiKy.Model.Response.UserBehaviorResponse;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.UserBehaviorService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DoAnCuoiKy.Service.IService.IUserBehavior;

namespace DoAnCuoiKy.Controllers.UserBehavior
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RecommendationController(IRecommendationService recommendationService, IHttpContextAccessor httpContextAccessor)
        {
            _recommendationService = recommendationService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("for-me")]
        public async Task<BaseResponse<List<RecommendationResponse>>> GetSavedRecommendations([FromQuery]int topN=10)
        {
            var currentUserId = getCurrentUserId();
            BaseResponse<List<RecommendationResponse>> baseResponse = await _recommendationService.GetSavedRecommendations(currentUserId, topN);
            return baseResponse;
        }
        [HttpPost("{recommendationId}/click")]
        [Authorize]
        public async Task<IActionResult> TrackClick(Guid recommendationId)
        {
            try
            {
                var userId = getCurrentUserId();

                await _recommendationService.TrackClick(recommendationId, userId);

                return Ok(new { success = true, message = "Click tracked" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPost("{recommendationId}/view")]
        [Authorize]
        public async Task<IActionResult> TrackView(Guid recommendationId)
        {
            try
            {
                var userId = getCurrentUserId();

                await _recommendationService.TrackView(recommendationId, userId);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPost("train")]
        public async Task<IActionResult> ManualTrain()
        {
            try
            {
                var result = await _recommendationService.TrainingModel();

                return Ok(new
                {
                    success = true,
                    message = "Training complete",
                    data = new
                    {
                        finalRMSE = result.data.FinalRMSE,
                        trainingTimeSeconds = (result.data.EndTime - result.data.StartTime).TotalSeconds
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPost("generate")]
        [Authorize]
        public async Task<IActionResult> ManualGenerate()
        {
            try
            {
                var userId = getCurrentUserId();

                await _recommendationService.GenerateAndSaveRecommendationsForUser(userId);

                return Ok(new { success = true, message = "Recommendations generated" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPost("generate-all")]
        public async Task<IActionResult> ManualGenerateAll()
        {
            try
            {
                await _recommendationService.GenerateAndSaveRecommendationsForAllUser();

                return Ok(new { success = true, message = "Generated for all users" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        private Guid getCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not Authentiated");
            }
            var userId = user.FindFirst(JwtRegisteredClaimNames.Sub) ?? user.FindFirst(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new Exception("userId không tồn tại");
            }
            return Guid.Parse(userId.Value);
        }
    }
}
