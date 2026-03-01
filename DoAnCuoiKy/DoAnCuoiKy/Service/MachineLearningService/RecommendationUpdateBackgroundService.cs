

using DoAnCuoiKy.Service.IService.IUserBehavior;
using DoAnCuoiKy.Service.UserBehaviorService;

namespace DoAnCuoiKy.Service.MachineLearningService
{
    public class RecommendationUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public RecommendationUpdateBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            await TrainAndGenerate();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
                await TrainAndGenerate();
            }
        }
        private async Task TrainAndGenerate()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IRecommendationService>();
                await service.TrainingModel();
                await service.GenerateAndSaveRecommendationsForAllUser();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
