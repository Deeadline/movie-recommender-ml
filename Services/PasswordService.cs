using Recommend_Movie_System.Services.Interface;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recommend_Movie_System.Services
{
    public class PasswordService : IPasswordService
    {
        public bool VerifyPasswordHash(string password, IReadOnlyList<byte> storedHash, byte[] storedSalt)
        {
            if (storedHash.Count != 32) return false;
            if (storedSalt.Length != 64) return false;

            using (var hmac = new System.Security.Cryptography.HMACSHA256(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                if (computedHash.Where((t, i) => t != storedHash[i]).Any())
                {
                    return false;
                }
            }

            return true;
        }
    }
}