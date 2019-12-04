using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Repository;
using Recommend_Movie_System.Services.Interface;

using System.Collections.Generic;
using System.Linq;

namespace Recommend_Movie_System.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationContext _context;

        public GenreService(ApplicationContext context)
        {
            _context = context;
        }

        public List<GenreResponse> getAll()
        {
            return _context.genres.Select(g => new GenreResponse {id = g.id, name = g.name}).ToList();
        }
    }
}