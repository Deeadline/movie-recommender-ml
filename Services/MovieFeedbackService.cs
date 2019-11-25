using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Repository;
using Recommend_Movie_System.Services.Interface;
using System;
using System.Linq;

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
            throw new NotImplementedException();
        }

        public bool update(int movieId, int feedbackId, MovieFeedbackRequest request)
        {
            throw new NotImplementedException();
        }
    }
}