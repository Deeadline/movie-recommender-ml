using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recommend_Movie_System.Models;
using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Services.Interface;
using System.Collections.Generic;
using System.Net.Mime;

namespace Recommend_Movie_System.Controllers
{
    [Route("api/movies")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IMovieRecommendationService _movieRecommendationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MoviesController(IMovieService movieService, IHttpContextAccessor httpContextAccessor,
            IMovieRecommendationService movieRecommendationService
        )
        {
            _movieService = movieService;
            _httpContextAccessor = httpContextAccessor;
            _movieRecommendationService = movieRecommendationService;
        }

        /// <summary>
        /// Get recommendation for current user using matrix factorization algorithm
        /// </summary>
        /// <returns></returns>
        [HttpGet("recommendation")]
        [ProducesResponseType(typeof(MovieRecommendationResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public MovieRecommendationResponse GetMoviesRecommendation()
        {
            return _movieRecommendationService.getRecommendation(1);
        }

        /// <summary>
        /// Get all movies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<MovieResponse>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IList<MovieResponse> GetMovies()
        {
            return _movieService.getMovies();
        }

        /// <summary>
        /// Get movie with details
        /// </summary>
        /// <param name="movieId">Movie id</param>
        /// <returns></returns>
        [HttpGet("{movieId}")]
        [ProducesResponseType(typeof(MovieResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public MovieDetailResponse GetMovieById(int movieId)
        {
            return _movieService.getMovie(movieId, 1);
        }

        /// <summary>
        /// Add movie
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/movies
        ///     {
        ///         "title": "jumanji",
        ///         "releaseYear": 1995,
        ///         "genresIds": [1,2,3]
        ///     }
        /// 
        /// </remarks>
        /// <param name="request">Request body</param>
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public bool PostMovie([FromBody] MovieRequest request)
        {
            Response.StatusCode = 201;
            return _movieService.create(request);
        }

        /// <summary>
        /// Update movie
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/movies/1
        ///     {
        ///         "title": "jumanji",
        ///         "releaseYear": 1995,
        ///         "genresIds": [1,2,3]
        ///     }
        /// 
        /// </remarks>
        /// <param name="movieId">Movie id</param>
        /// <param name="request">Request body</param>
        [HttpPut("{movieId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public bool PutMovie(int movieId, [FromBody] MovieRequest request)
        {
            return _movieService.update(movieId, request);
        }

        /// <summary>
        /// Archive movie
        /// </summary>
        /// <param name="movieId">Movie id</param>
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{movieId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public bool DeleteMovie(int movieId)
        {
            return _movieService.delete(movieId);
        }
    }
}