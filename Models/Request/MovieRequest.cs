using System.Collections.Generic;

namespace Recommend_Movie_System.Models.Request
{
    public class MovieRequest
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int releaseYear { get; set; }
        public IList<int> genresIds { get; set; }
    }
}