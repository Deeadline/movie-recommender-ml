using System.Collections.Generic;

namespace Recommend_Movie_System.Models.Entity
{
    public class Genre : IEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public IList<MovieGenre> movieGenres { get; set; }
    }
}