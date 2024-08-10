using iskustvohr.Common;
using iskustvohr.Model;
using iskustvohr.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Web;
using iskustvohr.Service.Common;

namespace iskustvohr.Service
{
    public class ReviewService : IReviewService
    {
        protected IReviewRepository ReviewRepository { get; set; }
        protected IWebsiteRepository WebsiteRepository { get; set; }

        public ReviewService(IReviewRepository reviewRepository, IWebsiteRepository websiteRepository)
        {
            WebsiteRepository = websiteRepository;
            ReviewRepository = reviewRepository;
        }
        public async Task<PagedList<Review>> GetReviewsByUserIdAsync()
        {
            PagedList<Review> reviewsToReturn = await ReviewRepository.GetReviewsByUserIdAsync(new User { Id = Guid.Parse(HttpContext.Current.User.Identity.GetUserId())});
            if(reviewsToReturn == null)
            {
                return new PagedList<Review>(new List<Review>(), 0, 0);
            }

            foreach (Review review in reviewsToReturn.List)
            {
                if (review.Reply == null || review.Reply.Id == Guid.Empty)
                {
                    review.Reply = null;
                    continue;
                }

                review.Reply = await ReviewRepository.GetReplyByIdAsync((Reply)review.Reply);
            }
            return reviewsToReturn;
        }

        public async Task<PagedList<Review>> GetReviewsAsync(string url, Paging paging)
        {
            PagedList<Review> reviewsToReturn = await ReviewRepository.GetReviewsAsync(new Review { Website = await WebsiteRepository.GetWebsiteByUrlAsync(new Website { URL = url })}, paging);
            if(reviewsToReturn == null)
            {
                return new PagedList<Review>(new List<Review>(), 0, 0);
            }

            foreach(Review review in reviewsToReturn.List)
            {
                if (review.Reply == null || review.Reply.Id == Guid.Empty)
                {
                    review.Reply = null;
                    continue;
                }

                review.Reply = await ReviewRepository.GetReplyByIdAsync((Reply)review.Reply);
            }
            return reviewsToReturn;
        }

        public async Task<int> UpdateReviewAsync(Review review)
        {
            review.UpdatedAt = DateTime.UtcNow;
            review.UpdatedBy = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            Review reviewToCheck = await ReviewRepository.GetReviewByIdAsync(review);

            if(reviewToCheck.CreatedBy == review.UpdatedBy)
                return await ReviewRepository.UpdateReviewAsync(review);

            return 0;
        }

        public async Task<int> DeleteReviewAsync(Guid id, bool isReply = false)
        {
            if (id == Guid.Empty)
                return 0;

            Review reviewToCheck = await ReviewRepository.GetReviewByIdAsync(new Review { Id = id});

            if ((reviewToCheck.CreatedBy == Guid.Parse(HttpContext.Current.User.Identity.GetUserId())) || isReply)
                await ReviewRepository.DeleteReviewAsync(id);

            if (reviewToCheck.Reply == null)
            {
                return 1;
            }
            else
            {
                await DeleteReviewAsync(reviewToCheck.Reply.Id, true);
                return 1;
            }
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            Review reviewToCheck = await ReviewRepository.GetReviewByIdAsync(review);
            if(reviewToCheck != null)
            {
                return null;
            }

            review.Id = Guid.NewGuid();
            review.CreatedAt = DateTime.UtcNow;
            review.UpdatedAt = DateTime.UtcNow;
            review.CreatedBy = review.UpdatedBy = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            review.User = new User { Id = review.CreatedBy };
            review.IsActive = true;
            review.Reply = new Reply { Id = Guid.Empty };
            review.IsReview = true;
            review.Website = await WebsiteRepository.GetWebsiteByUrlAsync((Website)review.Website);
            return await ReviewRepository.CreateReviewAsync(review);
        }

        public async Task<int> CreateReplyAsync(Review review)
        {
            Guid reviewId = review.Id;
            Guid ReplyId = Guid.NewGuid();
            review.Id = ReplyId;
            review.CreatedAt = DateTime.UtcNow;
            review.UpdatedAt = DateTime.UtcNow;
            review.CreatedBy = review.UpdatedBy = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            review.User = new User { Id = review.CreatedBy };
            review.IsActive = true;
            review.Reply = new Reply { Id = Guid.Empty };
            review.IsReview = false;
            review.Website = await WebsiteRepository.GetWebsiteByUrlAsync((Website)review.Website);
            await ReviewRepository.CreateReviewAsync(review);

            return await ReviewRepository.CreateReplyAsync(reviewId, ReplyId);
        }
    }
}
