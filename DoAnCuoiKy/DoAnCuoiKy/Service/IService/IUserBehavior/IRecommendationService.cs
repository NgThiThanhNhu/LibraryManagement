using DoAnCuoiKy.Model.MachineLearningResponse;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.UserBehaviorResponse;

namespace DoAnCuoiKy.Service.IService.IUserBehavior
{
    public interface IRecommendationService
    { 
        Task<BaseResponse<TrainingResult>> TrainingModel();
        Task GenerateAndSaveRecommendationsForUser(Guid userId, int topN = 50);
        Task<BaseResponse<List<RecommendationResponse>>> GenerateAndSaveRecommendationsForAllUser();
        Task<BaseResponse<List<RecommendationResponse>>> GetSavedRecommendations(Guid userId, int topN);
        Task TrackClick(Guid recommendationId, Guid userId);
        Task TrackView(Guid recommendationId, Guid userId);
        Task TrackBorrow(Guid bookId, Guid userId);

    }
}
