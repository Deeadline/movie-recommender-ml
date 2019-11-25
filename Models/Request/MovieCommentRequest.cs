using System;

namespace Recommend_Movie_System.Models.Request
{
    public class MovieCommentRequest
    {
        public string comment { get; set; }
        public int userId { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
    }
}