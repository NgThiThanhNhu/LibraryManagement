using DoAnCuoiKy.Model.MachineLearning;
using DoAnCuoiKy.Model.MachineLearningResponse;
using Microsoft.AspNetCore.Routing.Constraints;

namespace DoAnCuoiKy.Service.MachineLearningService
{
    public class MatrixFactorization
    {
        private readonly int _numFactors;
        private readonly float _learningRate;
        private readonly float _regularization;
        private readonly int _iterations;
        private Dictionary<Guid, float[]> _userFactors;
        private Dictionary<Guid, float[]> _bookFactors;
        private Dictionary<Guid, float> _userBias;
        private Dictionary<Guid, float> _bookBias;
        private float _globalMean;
        private HashSet<Guid> _trainedUserIds;
        private HashSet<Guid> _trainedBookIds;
        public MatrixFactorization(int numFactors = 20, float learningRate = 0.01f, float regularization = 0.02f, int iterations = 100)
        {
            _numFactors = numFactors;
            _learningRate = learningRate;
            _regularization = regularization;
            _iterations = iterations;
        }
        private void Initialize(List<RatingData> ratingDatas)
        {
            Console.WriteLine("\nBuoc 1: Khoi tao weights...");
            _globalMean = ratingDatas.Average(r => r.Rating);
            Console.WriteLine($"   Global mean rating: {_globalMean:F2}");
            _trainedUserIds = ratingDatas.Select(r => r.UserId).Distinct().ToHashSet();
            _trainedBookIds = ratingDatas.Select(r=>r.BookId).Distinct().ToHashSet();
            Console.WriteLine($"   So users: {_trainedUserIds.Count}");
            Console.WriteLine($"   So books: {_trainedBookIds.Count}");
            Console.WriteLine($"   So factors: {_numFactors}");
            var random = new Random(42);
            _userFactors = new Dictionary<Guid, float[]>();
            _userBias = new Dictionary<Guid, float>();
            foreach(var userId in _trainedUserIds)
            {
                _userFactors[userId] = Enumerable.Range(0, _numFactors).Select(_=>(float)(random.NextDouble() * 0.1 - 0.05)).ToArray();
                _userBias[userId] = 0f; 
            }
            _bookFactors = new Dictionary<Guid, float[]>();
            _bookBias = new Dictionary<Guid, float>();
            foreach (var bookId in _trainedBookIds)
            {
                _bookFactors[bookId] = Enumerable.Range(0, _numFactors).Select(_ => (float)(random.NextDouble() * 0.1 - 0.05)).ToArray();
                _bookBias[bookId] = 0f;
            }
            Console.WriteLine("Khoi tao hoan tat\n");
        }
        private float PredictRating(Guid userId, Guid bookId)
        {
            if (!_userFactors.ContainsKey(userId) || !_bookFactors.ContainsKey(bookId))
            {
                return _globalMean;
            }

            float prediction = _globalMean + _userBias[userId] + _bookBias[bookId];
            var userVec = _userFactors[userId];
            var bookVec = _bookFactors[bookId];

            for (int k = 0; k < _numFactors; k++)
            {
                prediction += userVec[k] * bookVec[k];
            }

            return Math.Max(1f, Math.Min(5f, prediction));
        }
        private void UpdateWeights(Guid userId, Guid bookId, float error)
        {
            var userVec = _userFactors[userId];
            var bookVec = _bookFactors[bookId];
            _userBias[userId] += _learningRate * (error - _regularization * _userBias[userId]);
            _bookBias[bookId] += _learningRate * (error - _regularization * _bookBias[bookId]);
            for (int k=0; k<_numFactors; k++)
            {
                float userFactor = userVec[k];
                float bookFactor = bookVec[k];
                userVec[k] += _learningRate * (error * bookFactor - _regularization * userFactor);
                bookVec[k] += _learningRate * (error * userFactor - _regularization * bookFactor);
            }
        }
        public TrainingResult Train(List<RatingData> trainingData)
        {
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║  MATRIX FACTORIZATION - BAT DAU TRAINING  ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");
            Console.WriteLine($"Tong so ratings: {trainingData.Count}");
            var result = new TrainingResult { StartTime = DateTime.Now };
            Initialize(trainingData);
            float previousRmse = float.MaxValue;

            for (int epoch = 0; epoch < _iterations; epoch++)
            {
                float totalSquaredError = 0f;

                var shuffled = trainingData.OrderBy(_ => Guid.NewGuid()).ToList();
                foreach (var rating in shuffled)
                {
                    float prediction = PredictRating(rating.UserId, rating.BookId);
                    float error = rating.Rating - prediction;

                    totalSquaredError += error * error;
                    UpdateWeights(rating.UserId, rating.BookId, error);
                }

                float rmse = (float)Math.Sqrt(totalSquaredError / trainingData.Count);
                result.EpochErrors.Add(rmse);

                if (epoch == 0)
                {
                    Console.WriteLine(
                        $"Epoch {epoch + 1}/{_iterations} | RMSE = {rmse:F4}"
                    );
                }
                else
                {
                    float delta = rmse - previousRmse;
                    Console.WriteLine(
                        $"Epoch {epoch + 1}/{_iterations} | RMSE = {rmse:F4} | delta RMSE = {delta:F4}"
                    );
                }

                previousRmse = rmse;
            }

            result.EndTime = DateTime.Now;
            result.FinalRMSE = result.EpochErrors.Last();
            Console.WriteLine($"\nTraining hoan tat!");
            Console.WriteLine($"   Final RMSE: {result.FinalRMSE:F4}");
            Console.WriteLine($"   Thoi gian: {(result.EndTime - result.StartTime).TotalSeconds:F2}s\n");
            return result;
        }
        public float Predict(Guid userId, Guid bookId)
        {
            return PredictRating(userId, bookId);
        }
        public List<BookRecommendationResult> GetTopRecommendations(Guid userId, List<Guid> candidateBookIds, int topN = 10)
        {
            var prediction = candidateBookIds.Select(bookId => new BookRecommendationResult
            {
                BookId = bookId,
                PredictedScore = Predict(userId, bookId)
            }).OrderByDescending(r=>r.PredictedScore).Take(topN).ToList();
            return prediction;
        }
        public bool IsUserTrained(Guid userId) => _trainedUserIds?.Contains(userId) ?? false;
        public bool IsBookTrained(Guid bookId) => _trainedBookIds?.Contains(bookId) ?? false;
    }
}
