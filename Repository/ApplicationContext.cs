using Microsoft.EntityFrameworkCore;

using Recommend_Movie_System.Models.Entity;

namespace Recommend_Movie_System.Repository
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Movie> movies { get; set; }
        public DbSet<MovieFeedback> movieFeedbacks { get; set; }
        public DbSet<MovieComment> movieComments { get; set; }
        public DbSet<MovieGenre> movieGenres { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
    }
}