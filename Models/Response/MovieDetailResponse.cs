using System.Collections.Generic;

namespace Recommend_Movie_System.Models.Response
{
    public class MovieDetailResponse
    {
        public int id { get; set; }
        public string title { get; set; }
        public decimal averageRate { get; set; }
        public int releaseYear { get; set; }
        public int numberOfVotes { get; set; }
        public MovieFeedbackResponse feedback { get; set; }
        public IList<int> genresIds { get; set; }
    }
}