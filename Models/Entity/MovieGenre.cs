namespace Recommend_Movie_System.Models.Entity
{
    public class MovieGenre
    {
        public int movieId { get; set; }
        public Movie movie { get; set; }
        public int genreId { get; set; }
        public Genre genre { get; set; }
    }
}