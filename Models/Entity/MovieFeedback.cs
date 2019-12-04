namespace Recommend_Movie_System.Models.Entity
{
    public class MovieFeedback : IEntity
    {
        public int id { get; set; }
        public int? movieId { get; set; }
        public int userId { get; set; }
        public decimal rate { get; set; }
    }
}