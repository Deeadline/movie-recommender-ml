using Microsoft.ML.Data;

namespace Recommend_Movie_System.Models
{
    public class MovieRating
    {
        public int userId { get; set; }

        public int movieId { get; set; }
        [ColumnName("Label")] public float label { get; set; }
    }
}