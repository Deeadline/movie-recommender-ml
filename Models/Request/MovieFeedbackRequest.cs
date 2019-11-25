namespace Recommend_Movie_System.Models.Request
{
    public class MovieFeedbackRequest
    {
        public decimal rate { get; set; }
        public int userId { get; set; }
    }
}