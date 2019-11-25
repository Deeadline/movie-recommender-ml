using Recommend_Movie_System.Models.Entity;
using Recommend_Movie_System.Models.Request;

namespace Recommend_Movie_System.Services.Interface
{
    public interface IUserService
    {
        bool Register(RegistrationRequest request);
        User Get(int id);
    }
}