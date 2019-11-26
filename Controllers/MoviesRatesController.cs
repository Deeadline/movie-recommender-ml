using Microsoft.AspNetCore.Mvc;
using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Services.Interface;

namespace Recommend_Movie_System.Controllers
{
    [Route("api/movies/{movieId}/rates")]
    [ApiController]
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
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public bool PostMovieFeedback([FromRoute] int movieId, [FromBody] MovieFeedbackRequest request)
        {
            Response.StatusCode = 201;
            return _movieFeedbackService.create(movieId, request);
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
        /// <returns></returns>
        [HttpPut("{rateId}")]
        [ProducesResponseType(typeof(bool), 204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public bool PutMovieFeedback([FromRoute] int movieId, int rateId, [FromBody] MovieFeedbackRequest request)
        {
            Response.StatusCode = 204;
            return _movieFeedbackService.update(rateId, request);
        }
    }
}