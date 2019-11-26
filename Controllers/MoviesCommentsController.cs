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
//    [Authorize]
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
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<MovieCommentResponse>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IList<MovieCommentResponse> GetMovieComments([FromRoute] int movieId)
        {
            return _movieCommentService.getComments(movieId);
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
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public bool PostMovieComments([FromRoute] int movieId, [FromBody] MovieCommentRequest request)
        {
            Response.StatusCode = 201;
            return _movieCommentService.create(movieId, request);
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
        /// <returns></returns>
        [HttpPut("{commentId}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public bool PutMovieComment([FromRoute] int movieId, int commentId, [FromBody] MovieCommentRequest request)
        {
            return _movieCommentService.update(commentId, request);
        }

        /// <summary>
        /// Archive comment
        /// </summary>
        /// <param name="movieId">Movie id</param>
        /// <param name="commentId">Comment id</param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{commentId}")]
        [ProducesResponseType(typeof(bool), 204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public bool DeleteComment([FromRoute] int movieId, int commentId)
        {
            Response.StatusCode = 204;
            return _movieCommentService.delete(commentId);
        }
    }
}