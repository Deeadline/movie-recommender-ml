using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Repository;
using Recommend_Movie_System.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Recommend_Movie_System.Models.Entity;

namespace Recommend_Movie_System.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationContext _context;
        private readonly IMovieFeedbackService _feedbackService;

        public MovieService(ApplicationContext context,
            IMovieFeedbackService feedbackService)
        {
            _context = context;
            _feedbackService = feedbackService;
        }

        public IList<MovieResponse> getMovies()
        {
            return (from movie in _context.movies
                    join feedback in _context.movieFeedbacks
                        on movie.id equals feedback.movieId
                    where movie.archived.Equals(false)
                    select new
                    {
                        movieId = movie.id,
                        movie.title,
                        movie.releaseYear,
                        feedback.rate,
                    }).GroupBy(y => y.movieId).Take(500).ToList()
                .Select(f => new MovieResponse
                {
                    id = f.Key,
                    title = f.FirstOrDefault().title,
                    releaseYear = f.FirstOrDefault().releaseYear,
                    genresIds = _context.movieGenres.Where(y => y.movieId.Equals(f.Key)).Select(yy => yy.genreId)
                        .ToList(),
                    averageRate = f.Average(l => l.rate)
                }).ToList();
        }

        public MovieResponse getMovie(int movieId)
        {
            return (from movie in _context.movies
                    join feedback in _context.movieFeedbacks
                        on movie.id equals feedback.movieId
                    where movie.id.Equals(movieId)
                    select new
                    {
                        movieId = movie.id,
                        movie.title,
                        movie.releaseYear,
                        feedback.rate,
                    }).GroupBy(y => y.movieId)
                .Select(f => new MovieResponse
                {
                    id = f.Key,
                    title = f.FirstOrDefault().title,
                    releaseYear = f.FirstOrDefault().releaseYear,
                    genresIds = _context.movieGenres.Where(y => y.movieId.Equals(f.Key)).Select(yy => yy.genreId)
                        .ToList(),
                    averageRate = f.Average(l => l.rate)
                }).FirstOrDefault();
        }

        public MovieDetailResponse getMovieWithDetails(int movieId, int userId)
        {
            return (from movie in _context.movies
                    join feedback in _context.movieFeedbacks
                        on movie.id equals feedback.movieId
                    where movie.id.Equals(movieId)
                    select new
                    {
                        movieId = movie.id,
                        movie.title,
                        movie.releaseYear,
                        feedback.rate,
                    }).GroupBy(y => y.movieId)
                .Select(f => new MovieDetailResponse
                {
                    id = f.Key,
                    title = f.First().title,
                    releaseYear = f.First().releaseYear,
                    averageRate = f.Average(l => l.rate),
                    genresIds = _context.movieGenres.Where(y => y.movieId.Equals(f.Key)).Select(yy => yy.genreId)
                        .ToList(),
                    numberOfVotes = f.Count(),
                    feedback = _feedbackService.getFeedback(movieId, userId)
                }).FirstOrDefault();
        }

        public bool create(MovieRequest request)
        {
            if (_context.movies.FirstOrDefault(m => m.title.Equals(request.title)) != null)
            {
                throw new Exception($"Movie with provided title: {request.title} is already taken");
            }

            var newModel = new Movie
            {
                title = request.title,
                releaseYear = request.releaseYear,
            };
            foreach (var genre in _context.genres.Where(g => request.genresIds.Contains(g.id)))
            {
                var movieGenre = new MovieGenre
                {
                    genre = genre,
                    movie = newModel
                };
                newModel.movieGenres.Add(movieGenre);
            }

            _context.movies.Add(newModel);
            var result = _context.SaveChanges() > 0;
            request.id = newModel.id;
            return result;
        }

        public bool update(int movieId, MovieRequest request)
        {
            var originalModel = _context.movies.FirstOrDefault(m => m.id.Equals(movieId));
            var parsedModel = new Movie
            {
                title = request.title,
                releaseYear = request.releaseYear,
            };

            if (originalModel is null)
            {
                throw new Exception("Model not found");
            }

            foreach (var genre in _context.genres.Where(g => request.genresIds.Contains(g.id)))
            {
                var movieGenre = new MovieGenre
                {
                    genre = genre,
                    movie = parsedModel
                };
                parsedModel.movieGenres.Add(movieGenre);
            }

            parsedModel.id = originalModel.id;

            _context.Entry(originalModel).CurrentValues.SetValues(parsedModel);
            return _context.SaveChanges() > 0;
        }

        public bool delete(int movieId)
        {
            var modelToArchive = _context.movies.FirstOrDefault(m => m.id.Equals(movieId));
            if (modelToArchive is null)
            {
                throw new Exception("Model not found");
            }

            modelToArchive.archived = true;
            _context.movies.Update(modelToArchive);
            return _context.SaveChanges() > 0;
        }
    }
}