using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Repository;
using Recommend_Movie_System.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Recommend_Movie_System.Models.Entity;

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
                where comment.movieId.Equals(movieId) && comment.archived.Equals(false)
                select new MovieCommentResponse
                {
                    id = comment.id,
                    description = comment.description,
                    movieId = comment.movieId,
                    userId = comment.userId,
                    createdAt = comment.createdAt,
                    updatedAt = comment.updatedAt
                }).ToList();
        }

        public bool create(int movieId, MovieCommentRequest request)
        {
            if (_context.movieComments.FirstOrDefault(
                    m => m.movieId.Equals(movieId) && m.userId.Equals(request.userId)) != null)
            {
                throw new Exception("Feedback for this movie is given!");
            }

            var newModel = new MovieComment
            {
                movieId = movieId,
                description = request.description,
                userId = request.userId,
            };

            _context.movieComments.Add(newModel);
            return _context.SaveChanges() > 0;
        }

        public bool update(int commentId, MovieCommentRequest request)
        {
            var originalModel = _context.movieComments.FirstOrDefault(m => m.id.Equals(commentId));
            var parsedModel = new MovieComment
            {
                description = request.description,
                updatedAt = request.updatedAt
            };

            if (originalModel is null)
            {
                throw new Exception("Model not found");
            }

            parsedModel.id = originalModel.id;

            _context.Entry(originalModel).CurrentValues.SetValues(parsedModel);
            return _context.SaveChanges() > 0;
        }

        public bool delete(int commentId)
        {
            var modelToArchive = _context.movieComments.FirstOrDefault(m => m.id.Equals(commentId));
            if (modelToArchive is null)
            {
                throw new Exception("Model not found");
            }

            modelToArchive.archived = true;
            _context.movieComments.Update(modelToArchive);
            return _context.SaveChanges() > 0;
        }
    }
}