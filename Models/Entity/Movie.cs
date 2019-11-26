using System.Collections.Generic;

namespace Recommend_Movie_System.Models.Entity
{
    public class Movie : IEntity
    {
        public int id { get; set; }
        public string title { get; set; }
        public int releaseYear { get; set; }
        public bool archived { get; set; }
        public IList<MovieGenre> genres { get; set; }
        public IList<MovieFeedback> feedbacks { get; set; }
        public IList<MovieComment> comments { get; set; }

        public Movie()
        {
            archived = false;
            genres = new List<MovieGenre>();
            feedbacks = new List<MovieFeedback>();
            comments = new List<MovieComment>();
        }
    }
}