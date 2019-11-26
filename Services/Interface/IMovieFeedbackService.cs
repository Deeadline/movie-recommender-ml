using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Models.Response;

namespace Recommend_Movie_System.Services.Interface
{
    public interface IMovieFeedbackService
    {
        MovieFeedbackResponse getFeedback(int movieId, int userId);
        bool create(int movieId, MovieFeedbackRequest request);
        bool update(int feedbackId, MovieFeedbackRequest request);
    }
}