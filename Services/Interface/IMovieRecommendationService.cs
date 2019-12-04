using System.Collections.Generic;
using System.Threading.Tasks;
using Recommend_Movie_System.Models.Response;

namespace Recommend_Movie_System.Services.Interface
{
    public interface IMovieRecommendationService
    {
        Task<IList<MovieResponse>> getRecommendation(int userId);
    }
}