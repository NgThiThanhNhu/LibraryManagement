using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.UserBehavior;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.UserBehavior;
using DoAnCuoiKy.Model.MachineLearning;
using DoAnCuoiKy.Model.MachineLearningResponse;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.UserBehaviorResponse;
using DoAnCuoiKy.Service.IService.IUserBehavior;
using DoAnCuoiKy.Service.MachineLearningService;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;

namespace DoAnCuoiKy.Service.UserBehaviorService
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ApplicationDbContext _context;
        private MatrixFactorization _model;
        private DateTime _lastTrainedAt;
        private readonly object _lockObj = new object();
        private const string MODEL_VERSION = "v1.0";
        public RecommendationService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResponse<List<RecommendationResponse>>> GenerateAndSaveRecommendationsForAllUser()
        {
            if (_model == null)
            {
                await TrainingModel();
            }
            var userIds = await _context.librarians.Where(x => x.RoleId.ToString() == "27A91ADE-F847-4D20-AE11-EDDCB0D96D27").Select(x => x.Id).ToListAsync();
            int successCount = 0;
            int errorCount = 0;
            foreach (var userId in userIds)
            {
                try
                {
                    await GenerateAndSaveRecommendationsForUser(userId.Value, topN: 50);
                    successCount++;
                    if (successCount % 10 == 0)
                    {
                        return new BaseResponse<List<RecommendationResponse>>
                        {
                            IsSuccess = true,
                            message = $"   Progress: {successCount}/{userIds.Count}"
                        };

                    }
                }
                catch (Exception ex)
                {
                    errorCount++;
                    return new BaseResponse<List<RecommendationResponse>>
                    {
                        IsSuccess = false,
                        message = $"Error for User {userId}"
                    };
                }
            }
            return new BaseResponse<List<RecommendationResponse>>
            {
                IsSuccess = true,
                message = $"Complete! Success: {successCount}/{userIds.Count}, Errors: {errorCount}"
            };
        }
        private async Task<List<BookRecommendationResult>> GetMLPredictions(Guid userId, int topN)
        {
            var interactionBookIds = new HashSet<Guid>();
            var rateBookIds = await _context.userBookInteractions.Where(i => i.UserId == userId && i.Rating > 0).Select(i => i.BookId).ToListAsync();
            interactionBookIds.UnionWith(rateBookIds);
            var readBookIds = await _context.userReadingSessions.Where(s=>s.UserId == userId).Select(s=>s.BookId).Distinct().ToListAsync();
            var candidateBookIds = await _context.books.Where(b=>!interactionBookIds.Contains(b.Id)).Select(b => b.Id).ToListAsync();
            if (candidateBookIds.Count == 0)
            {
                return new List<BookRecommendationResult>();
            }
            var predictions = _model.GetTopRecommendations(userId, candidateBookIds, topN);
            return predictions;
        }
        public async Task GenerateAndSaveRecommendationsForUser(Guid userId, int topN = 50)
        {
            if (_model == null)
            {
                throw new Exception("Model chưa được train!");
            }
            var mlRecommendations = await GetMLPredictions(userId, topN);
            if (mlRecommendations.Count == 0)
            {
                throw new Exception($"No recommendations for User {userId}");
            }
            var oldRecommendations = await _context.bookRecommendations.Where(r => r.UserId == userId && r.RecommendationType == Model.Enum.UserBehavior.RecommendationType.MatrixFactorization).ToListAsync();
            if (oldRecommendations.Any())
            {
                _context.bookRecommendations.RemoveRange(oldRecommendations);
            }
            var now = DateTime.Now;
            var expiresAt = now.AddMinutes(3);
            var newRecommendations = mlRecommendations.Select((rec, index) => new BookRecommendation
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BookId = rec.BookId,
                Score = (decimal)rec.PredictedScore,
                Rank = index + 1,
                Reason = "Dựa trên sở thích đọc sách của bạn",
                RecommendationType = Model.Enum.UserBehavior.RecommendationType.MatrixFactorization,
                CreateDate = now,
                ExpiresAt = expiresAt,
                ModelVersion = MODEL_VERSION,
                IsClicked = false,
                IsViewed = false,
                IsBorrowed = false,
                UpdateDate = now,
            }).ToList();
            await _context.bookRecommendations.AddRangeAsync(newRecommendations);
            await _context.SaveChangesAsync();
        }

        public async Task<BaseResponse<List<RecommendationResponse>>> GetSavedRecommendations(Guid userId, int topN)
        {
            BaseResponse<List<RecommendationResponse>> response = new BaseResponse<List<RecommendationResponse>>();
            var recommendations = await _context.bookRecommendations.Where(r => r.UserId == userId && r.RecommendationType == Model.Enum.UserBehavior.RecommendationType.MatrixFactorization).Include(r => r.Book).ThenInclude(b => b.BookAuthor).Include(r => r.Book).ThenInclude(b => b.Category).Include(r=>r.Book).ThenInclude(b=>b.bookFiles).ToListAsync();
            var filter = recommendations.Where(r => r.IsExpired() == false).OrderBy(r => r.Rank).Take(topN).ToList();
            if (!recommendations.Any())
            {
                await GenerateAndSaveRecommendationsForUser(userId);
                recommendations = await _context.bookRecommendations.Where(r => r.UserId == userId && r.RecommendationType == Model.Enum.UserBehavior.RecommendationType.MatrixFactorization).OrderBy(r => r.Rank).Take(topN).Include(r => r.Book).ThenInclude(b => b.BookAuthor).Include(r => r.Book).ThenInclude(b => b.Category).Include(r => r.Book).ThenInclude(b => b.bookFiles).ToListAsync();
            }
            List<RecommendationResponse> result = filter.Select(r=> new RecommendationResponse
            {
                RecommendationId = r.Id,
                BookId = r.BookId,
                Title = r.Book.Title,
                AuthorName = r.Book.BookAuthor.Name,
                CategoryName = r.Book.Category.Name,
                CoverImageUrl = r.Book.bookFiles.Select(x=>x.ImageUrl).First(),
                PredictedScore = (float)r.Score,
                Rank = r.Rank,
                Reason = r.Reason,
                GeneratedAt = r.CreateDate
            }).ToList();
            response.IsSuccess = true;
            response.message = "Lấy list đề xuất thành công";
            response.data = result;
            return response;
        }

        public async Task TrackBorrow(Guid bookId, Guid userId)
        {
            var recommendation = await _context.bookRecommendations.FirstOrDefaultAsync(r => r.BookId == bookId && r.UserId == userId && r.RecommendationType == RecommendationType.MatrixFactorization);
            if (recommendation != null && !recommendation.IsBorrowed)
            {
                recommendation.IsBorrowed = true;
                recommendation.BorrowedAt = DateTime.Now;
                recommendation.UpdateDate = DateTime.Now;
                _context.bookRecommendations.Update(recommendation);
                await _context.SaveChangesAsync();
            }
        }

        public async Task TrackClick(Guid recommendationId, Guid userId)
        {
            var recommendation = await _context.bookRecommendations.FirstOrDefaultAsync(r => r.Id == recommendationId && r.UserId == userId);
            if (recommendation == null)
            {
                throw new Exception("Đề xuất không tồn tại");
            }
            if (!recommendation.IsViewed)
            {
                recommendation.IsViewed = true;
                recommendation.ViewedAt = DateTime.Now;
                recommendation.UpdateDate = DateTime.Now;
                _context.bookRecommendations.Update(recommendation);
                await _context.SaveChangesAsync();
            }
        }

        public async Task TrackView(Guid recommendationId, Guid userId)
        {
            var recommendation = await _context.bookRecommendations.FirstOrDefaultAsync(r => r.Id == recommendationId && r.UserId == userId);

            if (recommendation == null) return;

            if (!recommendation.IsViewed)
            {
                recommendation.IsViewed = true;
                recommendation.ViewedAt = DateTime.Now;
                recommendation.UpdateDate = DateTime.Now;
                _context.bookRecommendations.Update(recommendation);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<BaseResponse<TrainingResult>> TrainingModel()
        {
            var trainingData = await PrepareTrainingDataAsync();
            if (trainingData.Count < 10)
            {
                return new BaseResponse<TrainingResult>
                {
                    IsSuccess = false,
                    message = $"Không đủ dữ liệu! Hiện có {trainingData.Count} ratings, cần ít nhất 10."
                };
            }
            lock (_lockObj)
            {
                _model = new MatrixFactorization(
                    numFactors: 20,
                    learningRate: 0.01f,
                    regularization: 0.02f,
                    iterations: 100
                );
                var result = _model.Train(trainingData);
                _lastTrainedAt = DateTime.Now;
                return new BaseResponse<TrainingResult>
                {
                    IsSuccess = true,
                    data = result,
                    message = "Huấn luyện mô hình hoàn tất thành công."
                };
            }
        }
        private async Task<List<RatingData>> PrepareTrainingDataAsync()
        {
            var trainingData = new List<RatingData>();
            var ratings = await _context.userBookInteractions.Where(i => i.Rating > 0).Select(i => new RatingData
            {
                UserId = i.UserId,
                BookId = i.BookId,
                Rating = (float)i.Rating
            }).ToListAsync();
            trainingData.AddRange(ratings);
            var readingSession = await _context.userReadingSessions.Where(s => s.DurationSeconds > 300).GroupBy(s => new { s.UserId, s.BookId }).Select(g => new
            {
                g.Key.UserId,
                g.Key.BookId,
                TotalSeconds = g.Sum(s => s.DurationSeconds),
                SessionCount = g.Count(),
                AvgProgress = g.Average(s => (double)s.ReadingProgress),
                IsCompleted = g.Any(s => s.IsCompleted == true)
            }).ToListAsync();
            foreach (var session in readingSession)
            {
                if (!trainingData.Any(t=>t.UserId == session.UserId && t.BookId == session.BookId))
                {
                    float rating = CalculateImplicitRating(session.TotalSeconds, session.SessionCount, session.AvgProgress, session.IsCompleted);
                    trainingData.Add(new RatingData { 
                        UserId= session.UserId,
                        BookId= session.BookId,
                        Rating = rating
                    });
                }
            }
            return trainingData;
        }
        private float CalculateImplicitRating(int totalSeconds, int sessionCount, double avgProgress, bool isCompleted)
        {
            float rating = 0f;
            int totalMinutes = totalSeconds / 60;
            if (totalMinutes >= 180) rating += 2.5f;
            else if (totalMinutes >= 120) rating += 2.0f;
            else if (totalMinutes >= 60) rating += 1.5f;
            else if (totalMinutes >= 30) rating += 1.0f;
            else rating += 0.5f;
            if (sessionCount >= 5)
            {
                rating += 1.5f;
            }else if (sessionCount >= 3)
            {
                rating += 1.0f;
            }else if (sessionCount >= 2)
            {
                rating += 0.5f;
            }
            rating += (float)(avgProgress * 1.0);
            if (isCompleted)
            {
                rating += 0.5f;
            }
            return Math.Max(1f, Math.Min(5f, rating));
        }
    }
}
