using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recommend_Movie_System.Models;
using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Services.Interface;
using System.Collections.Generic;
using System.Net.Mime;

namespace Recommend_Movie_System.Controllers
{
    [Route("api/movies/{movieId}/comments")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public class MoviesCommentsController : ControllerBase
    {
        private readonly IMovieCommentService _movieCommentService;

        public MoviesCommentsController(IMovieCommentService movieCommentService)
        {
            _movieCommentService = movieCommentService;
        }

        /// <summary>
        /// Get comments for movie
        /// </summary>
        /// <param name="movieId">Movie id</param>
        /// <response code="200">Returns movie comments collection</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If unexpected error appear</response>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<MovieCommentResponse>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult GetMovieComments([FromRoute] int movieId)
        {
            return Ok(_movieCommentService.getComments(movieId));
        }

        /// <summary>
        /// Add comment to movie
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/movies/1/comments
        ///     {
        ///         "comment": "Good movie,
        ///         "userId": 1,
        ///         "createdAt": 2019-26-11
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
        [ProducesResponseType(typeof(List<MovieCommentResponse>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult PostMovieComments([FromRoute] int movieId, [FromBody] MovieCommentRequest request)
        {
            return StatusCode(201, _movieCommentService.create(movieId, request));
        }

        /// <summary>
        /// Update comment for movie by user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/movies/1/comments/1
        ///     {
        ///         "comment": "Good movie,
        ///         "userId": 1,
        ///         "updatedAt": 2019-26-11
        ///     }
        /// 
        /// </remarks>
        /// <param name="movieId">Movie id</param>
        /// <param name="commentId">Comment id</param>
        /// <param name="request">Request body</param>
        /// <response code="200">If successfully updated comment</response>
        /// <response code="400">If model not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If unexpected error appear</response>
        /// <returns></returns>
        [HttpPut("{commentId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult PutMovieComment([FromRoute] int movieId, int commentId,
            [FromBody] MovieCommentRequest request)
        {
            return Ok(_movieCommentService.update(commentId, request));
        }

        /// <summary>
        /// Archive comment
        /// </summary>
        /// <param name="movieId">Movie id</param>
        /// <param name="commentId">Comment id</param>
        /// <response code="204">If successfully archived comment</response>
        /// <response code="400">If model not found</response>
        /// <response code="401">If user is unauthorized</response>
        /// <response code="500">If unexpected error appear</response>
        /// <returns></returns>
        [HttpDelete("{commentId}")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult DeleteComment([FromRoute] int movieId, int commentId)
        {
            _movieCommentService.delete(commentId);
            return NoContent();
        }
    }
}