﻿using System;

namespace Recommend_Movie_System.Models.Response
{
    public class MovieCommentResponse
    {
        public int id { get; set; }
        public string comment { get; set; }
        public int userId { get; set; }
        public int movieId { get; set; }
        public DateTime createdAt { get; set; }
    }
}