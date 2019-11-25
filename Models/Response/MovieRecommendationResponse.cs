using System.Collections.Generic;

namespace Recommend_Movie_System.Models.Response
{
    public class MovieRecommendationResponse
    {
        private IList<MovieResponse> recommendedMovies { get; set; }
    }
}