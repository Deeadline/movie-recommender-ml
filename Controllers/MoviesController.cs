using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recommend_Movie_System.Models;
using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Services.Interface;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Recommend_Movie_System.Controllers
{
    [Route("api/movies")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;
        private readonly IMovieRecommendationService _movieRecommendationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MoviesController(IMovieService movieService, IHttpContextAccessor httpContextAccessor,
            IMovieRecommendationService movieRecommendationService,
            IGenreService genreService
        )
        {
            _movieService = movieService;
            _genreService = genreService;
            _httpContextAccessor = httpContextAccessor;
            _movieRecommendationService = movieRecommendationService;
        }

        /// <summary>
        /// Get recommendation for current user using matrix factorization algorithm
        /// </summary>
        /// <response code="200">Returns recommendation for user</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If unexpected error appear</response>
        /// <returns></returns>
        [HttpGet("recommendation")]
        [ProducesResponseType(typeof(List<MovieResponse>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMoviesRecommendation()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);
            Task<IList<MovieResponse>> recommendation = _movieRecommendationService.getRecommendation(userId);
            await Task.WhenAll(recommendation);
            IList<MovieResponse> data = recommendation.Status == TaskStatus.RanToCompletion
                ? recommendation.Result
                : null;
            return Ok(data);
        }

        /// <summary>
        /// Get all movies
        /// </summary>
        /// <response code="200">Returns movies collection</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If unexpected error appear</response>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<MovieResponse>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult GetMovies()
        {
            return Ok(_movieService.getMovies());
        }

        /// <summary>
        /// Get movie with details
        /// </summary>
        /// <param name="movieId">Movie id</param>
        /// <response code="200">Returns movie with details</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If unexpected error appear</response>
        /// <returns></returns>
        [HttpGet("{movieId}")]
        [ProducesResponseType(typeof(MovieResponse), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult GetMovieById(int movieId)
        {
            return Ok(_movieService.getMovieWithDetails(movieId, 1));
        }

        /// <summary>
        /// Get genres
        /// </summary>
        /// <response code="200">Returns genres</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If unexpected error appear</response>
        /// <returns></returns>
        [HttpGet("genres")]
        [ProducesResponseType(typeof(List<GenreResponse>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult GetGenres()
        {
            return Ok(_genreService.getAll());
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
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If model already exist in database or is invalid</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If unexpected error appear</response>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(typeof(MovieResponse), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult PostMovie([FromBody] MovieRequest request)
        {
            _movieService.create(request);
            return CreatedAtRoute("GetMovieById", new {movieId = request.id}, request);
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
        /// <response code="200">If successfully updated movie</response>
        /// <response code="400">If model not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If unexpected error appear</response>
        /// <returns></returns>
        [HttpPut("{movieId}")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult PutMovie(int movieId, [FromBody] MovieRequest request)
        {
            return Ok(_movieService.update(movieId, request));
        }

        /// <summary>
        /// Archive movie
        /// </summary>
        /// <param name="movieId">Movie id</param>
        /// <response code="204">If successfully archived movie</response>
        /// <response code="400">If model not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If unexpected error appear</response>
        /// <returns></returns>
        [HttpDelete("{movieId}")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult DeleteMovie(int movieId)
        {
            _movieService.delete(movieId);
            return NoContent();
        }
    }
}