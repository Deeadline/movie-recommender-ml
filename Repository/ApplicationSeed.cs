using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
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
                ApplicationContext db = serviceScope.ServiceProvider.GetService<ApplicationContext>();
                if (await db.Database.EnsureCreatedAsync())
                {
                    (List<PreparedMovie> preparedMovies, List<string> genres) = LoadMovieData();

                    if (!db.genres.Any())
                    {
                        var movieGenres = genres.Select(genre => new Genre {name = genre}).ToList();
                        db.genres.AddRange(movieGenres);
                        db.SaveChanges();
                    }

                    if (!db.movies.Any())
                    {
                        foreach (var preparedMovie in preparedMovies)
                        {
                            var movie = new Movie
                            {
                                releaseYear = preparedMovie.releaseYear,
                                title = preparedMovie.movieTitle
                            };
                            foreach (var genre in preparedMovie.genreNames)
                            {
                                var dbGenre = db.genres.FirstOrDefault(y => y.name == genre);
                                var movieGenre = new MovieGenre
                                {
                                    genre = dbGenre,
                                    movie = movie
                                };
                                movie.movieGenres.Add(movieGenre);
                                db.movies.Add(movie);
                            }
                        }

                        await db.SaveChangesAsync();
                    }

                    if (!db.users.Any())
                    {
                        List<User> users = new List<User>(10000);
                        byte[] passwordHash, passwordSalt;
                        Encryption.CreatePasswordHash("admin", out passwordHash, out passwordSalt);
                        users.Add(new User
                        {
                            email = "admin@example.com",
                            firstName = "admin",
                            lastName = "admin",
                            passwordSalt = passwordSalt,
                            passwordHash = passwordHash,
                            role = Role.Admin
                        });
                        for (int i = 2; i <= 610; i++)
                        {
                            Encryption.CreatePasswordHash($"test{i}", out passwordHash, out passwordSalt);
                            users.Add(new User
                            {
                                email = $"t{i}@example.com",
                                firstName = $"t{i}",
                                lastName = $"tt{i}",
                                passwordSalt = passwordSalt,
                                passwordHash = passwordHash,
                                role = Role.User
                            });
                        }

                        db.users.AddRange(users);
                        db.SaveChanges();
                    }

                    if (!db.movieFeedbacks.Any())
                    {
                        Random rnd = new Random();
                        string path = "E:\\ml-latest-small\\ratings.csv";
                        int moviesSize = db.movies.Count();
                        using (var streamReader = File.OpenText(path))
                        {
                            CsvReader reader = new CsvReader(streamReader);
                            foreach (var r in reader.GetRecords<Rate>())
                            {
                                await db.movieFeedbacks.AddAsync(new MovieFeedback()
                                {
                                    movieId = rnd.Next(1, moviesSize),
                                    userId = r.userId,
                                    rate = rnd.Next(1, 10)
                                });
                            }
                        }

                        await db.SaveChangesAsync();
                    }
                }
            }
        }

        private static (List<PreparedMovie>, List<string>) LoadMovieData()
        {
            string path = "E:\\ml-latest-small\\movies.csv";
            List<string> genres = new List<string>(10000);
            List<PreparedMovie> prepared = new List<PreparedMovie>(10000);
            try
            {
                using (Stream fileReader = File.OpenRead(path))
                using (StreamReader reader = new StreamReader(fileReader))
                {
                    bool header = true;
                    string line = string.Empty;
                    int index = 0;
                    while (!reader.EndOfStream)
                    {
                        if (header)
                        {
                            line = reader.ReadLine();
                            header = false;
                        }

                        line = reader.ReadLine();
                        string[] fields = line.Split(',');
                        int lastindexOf = fields[0].LastIndexOf(' ');
                        string[] movieFields =
                        {
                            fields[0].Substring(0, lastindexOf),
                            fields[0].Substring(lastindexOf + 1)
                        };
                        string[] genreFields = fields[1].Split('|');

                        string movieTitle = movieFields[0];
                        int releaseYear = int.Parse(movieFields[1].Trim('(', ')'));
                        prepared.Add(new PreparedMovie
                        {
                            releaseYear = releaseYear,
                            genreNames = genreFields.ToList(),
                            movieTitle = movieTitle
                        });
                        genres.AddRange(genreFields);
                        genres = genres.Distinct().ToList();

                        index++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(prepared.Count);
                throw;
            }

            return (prepared, genres);
        }
    }

    class PreparedMovie
    {
        public string movieTitle { get; set; }
        public int releaseYear { get; set; }
        public List<string> genreNames { get; set; }

        public PreparedMovie()
        {
            genreNames = new List<string>();
        }
    }

    class Rate
    {
        public int userId { get; set; }
        public int movieId { get; set; }
    }
}