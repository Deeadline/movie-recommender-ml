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

        public MovieCommentResponse create(int movieId, MovieCommentRequest request)
        {
            var newModel = new MovieComment
            {
                movieId = movieId,
                description = request.description,
                userId = request.userId,
            };

            _context.movieComments.Add(newModel);
            _context.SaveChanges();
            return new MovieCommentResponse
            {
                id = newModel.id,
                createdAt = newModel.createdAt,
                description = newModel.description,
                movieId = movieId,
                updatedAt = newModel.updatedAt,
                userId = newModel.userId
            };
        }

        public MovieCommentResponse update(int commentId, MovieCommentRequest request)
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
            _context.SaveChanges();
            return new MovieCommentResponse
            {
                id = originalModel.id,
                createdAt = originalModel.createdAt,
                description = originalModel.description,
                movieId = originalModel.movieId,
                updatedAt = originalModel.updatedAt,
                userId = originalModel.userId
            };
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