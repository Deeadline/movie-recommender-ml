using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Repository;
using Recommend_Movie_System.Services.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Recommend_Movie_System.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;

        public AuthenticationService(ApplicationContext context, IConfiguration configuration,
            IPasswordService passwordService)
        {
            _context = context;
            _configuration = configuration;
            _passwordService = passwordService;
        }

        public string Authenticate(LoginRequest request)
        {
            var user = _context.users.FirstOrDefault(u => u.email == request.email);

            if (user == null)
                throw new Exception("Username does not exist");

            if (!_passwordService.VerifyPasswordHash(request.password, user.passwordHash, user.passwordSalt))
                throw new Exception("Invalid password");

            var nameClaim = new Claim(type: ClaimTypes.Name, user.id.ToString());
            var roleClaim = new Claim(type: ClaimTypes.Role, user.role);

            return CreateToken(nameClaim, roleClaim);
        }

        public StringValues CreateToken(params Claim[] claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWT").GetSection("SecretKey").Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}