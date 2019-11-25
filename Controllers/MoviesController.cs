using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recommend_Movie_System.Models;
using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Services.Interface;

namespace Recommend_Movie_System.Controllers
{
    [Route("api/movies")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IMovieCommentService _movieCommentService;
        private readonly IMovieFeedbackService _movieFeedbackService;
        private readonly IMovieRecommendationService _movieRecommendationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MoviesController(IMovieService movieService, IHttpContextAccessor httpContextAccessor,
            IMovieCommentService movieCommentService,
            IMovieFeedbackService movieFeedbackService,
            IMovieRecommendationService movieRecommendationService
        )
        {
            _movieService = movieService;
            _httpContextAccessor = httpContextAccessor;
            _movieCommentService = movieCommentService;
            _movieFeedbackService = movieFeedbackService;
            _movieRecommendationService = movieRecommendationService;
        }

        /// <summary>
        /// Get recommendation for current user using matrix factorization algorithm
        /// </summary>
        /// <returns></returns>
        [HttpGet("recommendation")]
        public MovieRecommendationResponse GetMoviesRecommendation()
        {
            return _movieRecommendationService.getRecommendation(1);
        }

        /// <summary>
        /// Get all movies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IList<MovieResponse> GetMovies()
        {
            return _movieService.getMovies();
        }

        /// <summary>
        /// Get movie with details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public MovieDetailResponse GetMovieById(int id)
        {
            return _movieService.getMovie(id, 1);
        }

        /// <summary>
        /// Get comments for movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/comments")]
        public IList<MovieCommentResponse> GetMovieComments(int id)
        {
            return _movieCommentService.getComments(id);
        }

        /// <summary>
        /// Add rate to movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{id}/rate")]
        public bool PostMovieFeedback(int id, [FromBody] MovieFeedbackRequest request)
        {
            return _movieFeedbackService.create(id, request);
        }

        /// <summary>
        /// Add comment to movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{id}/comments")]
        public bool PostMovieComments(int id, [FromBody] MovieCommentRequest request)
        {
            return _movieCommentService.create(id, request);
        }

        /// <summary>
        /// Add movie
        /// </summary>
        /// <param name="request"></param>
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public bool PostMovie([FromBody] MovieRequest request)
        {
            return _movieService.create(request);
        }

        /// <summary>
        /// Update movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        [HttpPut("{id}")]
        public bool PutMovie(int id, [FromBody] MovieRequest request)
        {
            return _movieService.update(id, request);
        }

        /// <summary>
        /// Update rate for movie by user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rateId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}/rate/{rateId}")]
        public bool PutMovieFeedback(int id, int rateId, [FromBody] MovieFeedbackRequest request)
        {
            return _movieFeedbackService.update(id, rateId, request);
        }

        /// <summary>
        /// Update comment for movie by user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commentId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}/comments/{commentId}")]
        public bool PutMovieComment(int id, int commentId, [FromBody] MovieCommentRequest request)
        {
            return _movieCommentService.update(commentId, request);
        }

        /// <summary>
        /// Archive movie
        /// </summary>
        /// <param name="id"></param>
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public bool DeleteMovie(int id)
        {
            return _movieService.delete(id);
        }

        /// <summary>
        /// Archive comment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}/comments/{commentId}")]
        public bool DeleteComment(int id, int commentId)
        {
            return _movieCommentService.delete(commentId);
        }
    }
}