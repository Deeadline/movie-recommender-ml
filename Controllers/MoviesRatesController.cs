using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Services.Interface;

namespace Recommend_Movie_System.Controllers
{
    [Route("api/movies/{movieId}/rates")]
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    public class MoviesRatesController : ControllerBase
    {
        private readonly IMovieFeedbackService _movieFeedbackService;

        public MoviesRatesController(IMovieFeedbackService movieFeedbackService)
        {
            _movieFeedbackService = movieFeedbackService;
        }

        /// <summary>
        /// Add rate to movie
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/movies/1/rates
        ///     {
        ///         "rate": 5,
        ///         "userId": 1
        ///     }
        /// 
        /// </remarks>
        /// <param name="movieId">Movie id</param>
        /// <param name="request">Request body</param>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is not valid or already exist in database.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If unexpected error appear</response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult PostMovieFeedback([FromRoute] int movieId, [FromBody] MovieFeedbackRequest request)
        {
            return StatusCode(201, _movieFeedbackService.create(movieId, request));
        }

        /// <summary>
        /// Update rate for movie by user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/movies/1/rates/1
        ///     {
        ///         "rate": 5,
        ///         "userId": 1
        ///     }
        /// 
        /// </remarks>
        /// <param name="movieId">Movie id</param>
        /// <param name="rateId">Rate id</param>
        /// <param name="request">Request body</param>
        /// <response code="200">If successfully updated feedback</response>
        /// <response code="400">If model not found</response>
        /// <response code="401">If user is unauthorized</response>
        /// <response code="500">If unexpected error appear</response>
        /// <returns></returns>
        [HttpPut("{rateId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult PutMovieFeedback([FromRoute] int movieId, int rateId,
            [FromBody] MovieFeedbackRequest request)
        {
            return Ok(_movieFeedbackService.update(rateId, request));
        }
    }
}