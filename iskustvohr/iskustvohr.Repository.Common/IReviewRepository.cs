using iskustvohr.Common;
using iskustvohr.Model;
using System;
using System.Threading.Tasks;

namespace iskustvohr.Repository.Common
{
    public interface IReviewRepository
    {
        Task<Review> CreateReviewAsync(Review review);
        Task<PagedList<Review>> GetReviewsAsync(Review review, Paging paging);
        Task<Review> GetReviewByIdAsync(Review review);
        Task<Reply> GetReplyByIdAsync(Reply review);
        Task<int> CreateReplyAsync(Guid ReviewId, Guid ReplyId);
        Task<int> UpdateReviewAsync(Review review);
        Task<int> DeleteReviewAsync(Guid id);
        Task<PagedList<Review>> GetReviewsByUserIdAsync(User user);

    }
}