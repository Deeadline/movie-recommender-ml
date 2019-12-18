namespace Recommend_Movie_System.Models.Response
{
    public class MovieFeedbackResponse
    {
        public int id { get; set; }
        public int userId { get; set; }
        public decimal rate { get; set; }
    }
}