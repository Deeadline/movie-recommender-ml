using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Services.Interface;

namespace Recommend_Movie_System.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get user information
        /// </summary>
        /// <response code="200">Returns user information</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If unexpected error appear</response>
        /// <returns>User information</returns>
        [HttpGet("current")]
        public UserResponse Get()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);
            return _userService.Get(userId);
        }
    }
}