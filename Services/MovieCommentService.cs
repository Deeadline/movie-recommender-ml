using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Repository;
using Recommend_Movie_System.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Recommend_Movie_System.Services
{
    public class MovieCommentService : IMovieCommentService
    {
        private readonly ApplicationContext _context;

        public MovieCommentService(ApplicationContext context)
        {
            _context = context;
        }

        public IList<MovieCommentResponse> getComments(int movieId)
        {
            return (from comment in _context.movieComments
                where comment.movieId == movieId
                select new MovieCommentResponse()
                {
                    id = comment.id,
                    comment = comment.comment,
                    movieId = comment.movieId,
                    userId = comment.userId,
                    createdAt = comment.createdAt
                }).ToList();
        }

        public bool create(int movieId, MovieCommentRequest request)
        {
            throw new NotImplementedException();
        }

        public bool update(int commentId, MovieCommentRequest request)
        {
            throw new NotImplementedException();
        }

        public bool delete(int commentId)
        {
            throw new NotImplementedException();
        }
    }
}