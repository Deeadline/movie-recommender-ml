using System.Collections.Generic;

namespace Recommend_Movie_System.Models.Entity
{
    public class Movie : IEntity
    {
        public int id { get; set; }
        public string title { get; set; }
        public int releaseYear { get; set; }
        public IList<MovieGenre> genres { get; set; }
        public IList<MovieFeedback> feedbacks { get; set; }
        public IList<MovieComment> comments { get; set; }
    }
}