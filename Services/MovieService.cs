using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Repository;
using Recommend_Movie_System.Services.Interface;

using System;
using System.Collections.Generic;
using System.Linq;

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
                    join genre in _context.movieGenres
                        on movie.id equals genre.movieId
                    select new
                    {
                        movieId = movie.id,
                        movie.title,
                        movie.releaseYear,
                        feedback.rate,
                        genreId = genre.id
                    }).GroupBy(y => y.movieId)
                .Select(f => new MovieResponse
                {
                    id = f.Key,
                    title = f.FirstOrDefault().title,
                    releaseYear = f.FirstOrDefault().releaseYear,
                    genresIds = f.Select(y => y.genreId).Distinct().ToList(),
                    averageRate = f.Average(l => l.rate)
                }).ToList();
        }

        public MovieDetailResponse getMovie(int movieId, int userId)
        {
            return (from movie in _context.movies
                    join feedback in _context.movieFeedbacks
                        on movie.id equals feedback.movieId
                    join genre in _context.movieGenres
                        on movie.id equals genre.movieId
                    where movie.id == movieId
                    select new
                    {
                        movieId = movie.id,
                        movie.title,
                        movie.releaseYear,
                        feedback.rate,
                        genreId = genre.id,
                    }).GroupBy(y => y.movieId)
                .Select(f => new MovieDetailResponse()
                {
                    id = f.Key,
                    title = f.First().title,
                    releaseYear = f.First().releaseYear,
                    genresIds = f.Select(y => y.genreId).Distinct().ToList(),
                    averageRate = f.Average(l => l.rate),
                    numberOfVotes = f.Count(),
                    feedback = _feedbackService.getFeedback(movieId, userId)
                }).FirstOrDefault();
        }

        public bool create(MovieRequest request)
        {
            throw new NotImplementedException();
        }

        public bool update(int movieId, MovieRequest request)
        {
            throw new NotImplementedException();
        }

        public bool delete(int movieId)
        {
            throw new NotImplementedException();
        }
    }
}