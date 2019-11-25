using Recommend_Movie_System.Models.Response;

namespace Recommend_Movie_System.Services.Interface
{
    public interface IMovieRecommendationService
    {
        MovieRecommendationResponse getRecommendation(int userId);
    }
}