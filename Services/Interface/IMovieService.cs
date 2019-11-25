using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Models.Response;

using System.Collections.Generic;

namespace Recommend_Movie_System.Services.Interface
{
    public interface IMovieService
    {
        IList<MovieResponse> getMovies();
        MovieDetailResponse getMovie(int movieId, int userId);
        bool create(MovieRequest request);
        bool update(int movieId, MovieRequest request);
        bool delete(int movieId);
    }
}