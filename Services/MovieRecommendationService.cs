using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Trainers.Recommender;
using Recommend_Movie_System.Models;
using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Repository;
using Recommend_Movie_System.Services.Interface;
using System;
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
                Console.WriteLine("EVALUATING THE MODEL");
                IDataView prediction = model.Transform(testData);
                var metrics =
                    _mlContext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");
                Console.WriteLine("Me" + metrics.MeanSquaredError);
                Console.WriteLine("L1" + metrics.MeanAbsoluteError);
                Console.WriteLine("L2" + metrics.MeanSquaredError);
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
                }).Take(10000).ToList();
            var testData = (from m in _context.movieFeedbacks
                select new MovieRating
                {
                    movieId = m.movieId.Value,
                    userId = m.userId,
                    label = (float) m.rate
                }).Take(500).ToList();
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
                NumberOfIterations = 40,
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
            Console.WriteLine("TRAINING MODEL");
            return pipeline.Fit(trainingModel);
        }

        private IList<MovieResponse> getPredictions(
            PredictionEngine<MovieRating, MovieRatingPrediction> predictionEngine, int userId)
        {
            IQueryable<MovieRatingPrediction> top5 = _context.movies
                .Select(m => new {m, p = predictionEngine.Predict(new MovieRating {userId = userId, movieId = m.id})})
                .OrderByDescending(@t => @t.p.score)
                .Select(@t => new MovieRatingPrediction {label = @t.m.id, score = @t.p.score})
                .Take(5);
            var recommendedMovies =
                top5.Select(movie => _movieService.getMovie((int) movie.label)).ToList();
            foreach (var t in top5)
                Console.WriteLine(
                    $"Score:{t.score}\tMovie: {recommendedMovies.FirstOrDefault(y => y.id == (int) t.label)?.title}");
            return recommendedMovies;
        }
    }
}