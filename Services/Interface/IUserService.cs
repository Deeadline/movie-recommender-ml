using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Models.Response;

namespace Recommend_Movie_System.Services.Interface
{
    public interface IUserService
    {
        bool Register(RegistrationRequest request);
        UserResponse Get(int id);
    }
}