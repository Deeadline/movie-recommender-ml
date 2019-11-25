using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recommend_Movie_System.Helpers;
using Recommend_Movie_System.Models;
using Recommend_Movie_System.Models.Entity;

namespace Recommend_Movie_System.Repository
{
    public class ApplicationSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ApplicationContext>();
                if (await db.Database.EnsureCreatedAsync())
                {
                    if (db.users.FirstOrDefault(u => u.role.Equals("Admin")) is null)
                    {
                        Encryption.CreatePasswordHash("admin", out byte[] passwordHash, out byte[] passwordSalt);
                        await db.users.AddAsync(new User()
                        {
                            email = "admin@example.com",
                            firstName = "admin",
                            lastName = "admin",
                            passwordSalt = passwordSalt,
                            passwordHash = passwordHash,
                            role = Role.Admin
                        });
                        db.SaveChanges();
                    }

                    if (!db.movieGenres.Any())
                    {
                        var adventure = new MovieGenre() {name = "Adventure"};
                        var comedy = new MovieGenre() {name = "Comedy"};
                        var fantasy = new MovieGenre() {name = "Fantasy"};
                        var romance = new MovieGenre() {name = "Romance"};
                        var animation = new MovieGenre() {name = "Animation"};
                        var thriller = new MovieGenre() {name = "Thriller"};
                        var drama = new MovieGenre() {name = "Drama"};
                        var horror = new MovieGenre() {name = "Horror"};
                        db.movieGenres.AddRange(adventure, comedy, fantasy, romance, animation, thriller, drama,
                            horror);
                        db.SaveChanges();
                    }

                    if (!db.movies.Any())
                    {
                        var toyStory = new Movie
                        {
                            title = "Toy Story", releaseYear = 1995,
                            genres = db.movieGenres.Where(y => y.name == "Animation" || y.name == "Adventure").ToList()
                        };
                        var jumanji = new Movie()
                        {
                            title = "Jumanji", releaseYear = 1995,
                            genres = db.movieGenres.Where(y => y.name == "Fantasy" || y.name == "Adventure").ToList()
                        };
                        var sabrina = new Movie()
                        {
                            title = "Sabrina", releaseYear = 1995,
                            genres = db.movieGenres.Where(y => y.name == "Comedy" || y.name == "Romance").ToList()
                        };
                        db.movies.AddRange(toyStory, jumanji, sabrina);
                        db.SaveChanges();
                    }

                    if (!db.movieFeedbacks.Any())
                    {
                        var feedbacks = new List<MovieFeedback>();
                        Random rnd = new Random();
                        for (int i = 0; i < 10; i++)
                        {
                            feedbacks.Add(new MovieFeedback()
                            {
                                movieId = db.movies.Single(y => y.title == "Toy Story").id,
                                rate = rnd.Next(1, 10),
                                userId = db.users.Single(y => y.role == Role.Admin).id
                            });
                            feedbacks.Add(new MovieFeedback()
                            {
                                movieId = db.movies.Single(y => y.title == "Jumanji").id,
                                rate = rnd.Next(1, 10),
                                userId = db.users.Single(y => y.role == Role.Admin).id
                            });
                            feedbacks.Add(new MovieFeedback()
                            {
                                movieId = db.movies.Single(y => y.title == "Sabrina").id,
                                rate = rnd.Next(1, 10),
                                userId = db.users.Single(y => y.role == Role.Admin).id
                            });
                        }

                        db.movieFeedbacks.AddRange(feedbacks);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}