using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Repository;
using Recommend_Movie_System.Services.Interface;
using System;
using System.Linq;
using Recommend_Movie_System.Models.Entity;

namespace Recommend_Movie_System.Services
{
    public class MovieFeedbackService : IMovieFeedbackService
    {
        private readonly ApplicationContext _context;

        public MovieFeedbackService(ApplicationContext context)
        {
            _context = context;
        }

        public MovieFeedbackResponse getFeedback(int movieId, int userId)
        {
            return (from feedback in _context.movieFeedbacks
                where feedback.movieId.Equals(movieId) && feedback.userId.Equals(userId)
                select new MovieFeedbackResponse()
                {
                    rate = feedback.rate,
                    userId = userId,
                }).FirstOrDefault();
        }

        public bool create(int movieId, MovieFeedbackRequest request)
        {
            if (_context.movieFeedbacks.FirstOrDefault(
                    m => m.movieId.Equals(movieId) && m.userId.Equals(request.userId)) != null)
            {
                throw new Exception($"Feedback for this movie is given!");
            }

            var newModel = new MovieFeedback
            {
                movieId = movieId,
                rate = request.rate,
                userId = request.userId
            };

            _context.movieFeedbacks.Add(newModel);
            return _context.SaveChanges() > 0;
        }

        public bool update(int feedbackId, MovieFeedbackRequest request)
        {
            var originalModel = _context.movieFeedbacks.FirstOrDefault(m => m.id.Equals(feedbackId));
            var parsedModel = new MovieFeedback
            {
                rate = request.rate,
            };

            if (originalModel is null)
            {
                throw new Exception("Model not found");
            }

            parsedModel.id = originalModel.id;

            _context.Entry(originalModel).CurrentValues.SetValues(parsedModel);
            return _context.SaveChanges() > 0;
        }
    }
}