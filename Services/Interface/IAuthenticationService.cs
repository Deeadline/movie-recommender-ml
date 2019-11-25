using Microsoft.Extensions.Primitives;

using Recommend_Movie_System.Models.Request;

using System.Security.Claims;

namespace Recommend_Movie_System.Services.Interface
{
    public interface IAuthenticationService
    {
        string Authenticate(LoginRequest login);
        StringValues CreateToken(params Claim[] claims);
    }
}