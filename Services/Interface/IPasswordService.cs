using System.Collections.Generic;

namespace Recommend_Movie_System.Services.Interface
{
    public interface IPasswordService
    {
        bool VerifyPasswordHash(string password, IReadOnlyList<byte> storedHash, byte[] storedSalt);
    }
}