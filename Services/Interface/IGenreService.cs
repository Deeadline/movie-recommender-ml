using Recommend_Movie_System.Models.Response;
using System.Collections.Generic;

namespace Recommend_Movie_System.Services.Interface
{
    public interface IGenreService
    {
        List<GenreResponse> getAll();
    }
}