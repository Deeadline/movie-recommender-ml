using System.Collections.Generic;

namespace Recommend_Movie_System.Models.Response
{
    public class MovieResponse
    {
        public int id { get; set; }
        public string title { get; set; }
        public int releaseYear { get; set; }
        public IList<int> genresIds { get; set; }
        public decimal averageRate { get; set; }
    }
}