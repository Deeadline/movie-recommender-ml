using Microsoft.ML.Data;

namespace Recommend_Movie_System.Models
{
    public class MovieRatingPrediction
    {
        [ColumnName("Label")]
        public float label { get; set; }
        [ColumnName("Score")]
        public float score { get; set; }
    }
}
