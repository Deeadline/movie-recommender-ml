namespace Recommend_Movie_System.Models.Entity
{
    public class MovieGenre : IEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? movieId { get; set; }
    }
}