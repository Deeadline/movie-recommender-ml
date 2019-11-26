using System;

namespace Recommend_Movie_System.Models.Entity
{
    public class MovieComment : IEntity
    {
        public int id { get; set; }
        public string description { get; set; }
        public int userId { get; set; }
        public int movieId { get; set; }
        public bool archived { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? updatedAt { get; set; }

        public MovieComment()
        {
            archived = false;
        }
    }
}