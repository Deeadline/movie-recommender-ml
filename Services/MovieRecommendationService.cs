using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Trainers.Recommender;
using Recommend_Movie_System.Models;
using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Repository;
using Recommend_Movie_System.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend_Movie_System.Services
{
    public class MovieRecommendationService : IMovieRecommendationService
    {
        private readonly ApplicationContext _context;
        private readonly MLContext _mlContext;
        private readonly IMovieService _movieService;

        public MovieRecommendationService(ApplicationContext context, IMovieService movieService)
        {
            _context = context;
            _mlContext = new MLContext();
            _movieService = movieService;
        }

        public Task<IList<MovieResponse>> getRecommendation(int userId)
        {
            return Task.Run(() =>
            {
                (IDataView trainingData, IDataView testData) = loadData();

                EstimatorChain<MatrixFactorizationPredictionTransformer> pipeline = getPipeline();
                ITransformer model = trainModel(pipeline, trainingData);

                IDataView predictions = model.Transform(testData);
                // RegressionMetrics metrics =
                //     _mlContext.Regression.Evaluate(predictions);
                PredictionEngine<MovieRating, MovieRatingPrediction> predictionEngine =
                    _mlContext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(model);
                return getPredictions(predictionEngine, userId);
            });
        }

        private (IDataView, IDataView) loadData()
        {
            var trainingData = (from m in _context.movieFeedbacks
                select new MovieRating
                {
                    movieId = m.movieId.Value,
                    userId = m.userId,
                    label = (float) m.rate
                }).Take(10).AsEnumerable();
            var testData = (from m in _context.movieFeedbacks
                select new MovieRating
                {
                    movieId = m.movieId.Value,
                    userId = m.userId,
                    label = (float) m.rate
                }).AsEnumerable();

            IDataView trainingDataView = _mlContext.Data.LoadFromEnumerable(trainingData);
            IDataView testDataView = _mlContext.Data.LoadFromEnumerable(testData);
            return (trainingDataView, testDataView);
        }

        private MatrixFactorizationTrainer.Options getOptions()
        {
            return new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "movieIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };
        }

        private EstimatorChain<MatrixFactorizationPredictionTransformer> getPipeline()
        {
            return _mlContext.Transforms.Conversion
                .MapValueToKey(
                    inputColumnName: "userId",
                    outputColumnName: "userIdEncoded")
                .Append(_mlContext.Transforms.Conversion.MapValueToKey(
                    inputColumnName: "movieId",
                    outputColumnName: "movieIdEncoded"))
                .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(getOptions()));
        }

        private ITransformer trainModel(
            EstimatorChain<MatrixFactorizationPredictionTransformer> pipeline, IDataView trainingModel)
        {
            return pipeline.Fit(trainingModel);
        }

        private IList<MovieResponse> getPredictions(
            PredictionEngine<MovieRating, MovieRatingPrediction> predictionEngine, int userId)
        {
            IQueryable<MovieRatingPrediction> top5 = (from m in _context.movies
                let p = predictionEngine.Predict(
                    new MovieRating
                    {
                        userId = userId,
                        movieId = m.id
                    })
                orderby p.score descending
                select new MovieRatingPrediction
                {
                    label = m.id,
                    score = p.score
                }).Take(5);
            var recommendedMovies =
                top5.Select(movie => _movieService.getMovie((int) movie.label)).ToList();
            return recommendedMovies;
        }
    }
}