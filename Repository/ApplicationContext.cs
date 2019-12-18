using System;
using System.Linq;
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
        public DbSet<Genre> genres { get; set; }
        public DbSet<MovieGenre> movieGenres { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieGenre>().HasKey(k => new {k.movieId, k.genreId});
            modelBuilder.Entity<MovieGenre>()
                .HasOne(m => m.movie)
                .WithMany(c => c.movieGenres)
                .HasForeignKey(fk => fk.movieId);
            modelBuilder.Entity<MovieGenre>()
                .HasOne(m => m.genre)
                .WithMany(c => c.movieGenres)
                .HasForeignKey(fk => fk.genreId);
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is MovieComment && (
                                e.State == EntityState.Added
                                || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((MovieComment) entityEntry.Entity).updatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((MovieComment) entityEntry.Entity).createdAt = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}