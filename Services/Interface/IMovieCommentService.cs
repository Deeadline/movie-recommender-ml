using System.Collections.Generic;
using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Models.Response;

namespace Recommend_Movie_System.Services.Interface
{
    public interface IMovieCommentService
    {
        IList<MovieCommentResponse> getComments(int movieId);
        MovieCommentResponse create(int movieId, MovieCommentRequest request);
        MovieCommentResponse update(int commentId, MovieCommentRequest request);
        bool delete(int commentId);
    }
}