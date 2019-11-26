using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Services.Interface;

namespace Recommend_Movie_System.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Produces(MediaTypeNames.Application.Json)]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public AuthenticationController(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        /// <summary>
        /// email user
        /// </summary>
        /// <param name="request">email and password</param>
        /// <returns>JWT token</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/auth/login
        ///     {
        ///         "email": "test@example.com",
        ///         "password": "test"
        ///     }
        /// </remarks>
        /// <response code="200">Returns the JWT Token</response>
        /// <response code="500">If unexpected error appear</response>
        [ProducesResponseType(typeof(SecurityToken), 200)]
        [ProducesResponseType(500)]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var authenticated = _authenticationService.Authenticate(request);
            return Ok(authenticated);
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="request">Member data</param>
        /// <returns>Returns register status</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/auth/register
        ///     {
        ///         "firstName": "test",
        ///         "lastName": "test",
        ///         "email": "test@example.com",
        ///         "password": "test"
        ///     }
        /// </remarks>
        /// <response code="200">Returns register status</response>
        /// <response code="500">If unexpected error appear</response>
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(500)]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistrationRequest request)
        {
            return Ok(_userService.Register(request));
        }
    }
}