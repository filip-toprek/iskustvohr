using iskustvohr.Common;
using iskustvohr.Model;
using System;
using System.Threading.Tasks;

namespace iskustvohr.Service.Common
{
    public interface IReviewService
    {
        Task<PagedList<Review>> GetReviewsAsync(string url, Paging paging);
        Task<int> CreateReplyAsync(Review review);
        Task<int> UpdateReviewAsync(Review review);
        Task<int> DeleteReviewAsync(Guid id, bool isReply = false);
        Task<PagedList<Review>> GetReviewsByUserIdAsync();
        Task<Review> CreateReviewAsync(Review review);
    }
}