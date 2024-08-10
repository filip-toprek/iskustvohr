using AutoMapper;
using iskustvohr.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using iskustvohr.WebApi.Models;
using iskustvohr.Model;
using iskustvohr.Common;

namespace iskustvohr.WebApi.Controllers
{
    public class ReviewController : ApiController
    {
        protected IReviewService ReviewService { get; set; }
        protected IMapper Mapper { get; set; }
        public ReviewController(IReviewService reviewService, IMapper mapper)
        {
            ReviewService = reviewService;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("Api/Reviews/User")]
        public async Task<HttpResponseMessage> GetReviewsByUserIdAsync()
        {
            PagedList<Review> reviews = await ReviewService.GetReviewsByUserIdAsync();
            PagedList<UserReviewDetails> reviewDetails = new PagedList<UserReviewDetails>(new List<UserReviewDetails>(), 0, 0);
            reviewDetails.TotalCount = reviews.TotalCount;
            reviewDetails.PageSize = reviews.PageSize;
            reviewDetails.PageCount = reviews.PageCount;


            reviewDetails.List = reviews.List
                    .Select(x => Mapper.Map<UserReviewDetails>(x))
                    .ToList();
            if (reviews == null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to get reviews");
            }

            float averageRating = 0;
            foreach (Review review in reviews.List)
            {
                if (review.ReviewScore != 0)
                {
                    averageRating += review.ReviewScore;
                }
                reviewDetails.AverageRating = averageRating / reviews.List.Count(x => x.ReviewScore != 0);
                reviewDetails.AverageRating = (float)Math.Round(reviewDetails.AverageRating, 2);
            }
            return Request.CreateResponse(HttpStatusCode.OK, reviewDetails);
        }

        [HttpGet]
        [Route("Api/Review/{url}")]
        public async Task<HttpResponseMessage> GetReviewsAsync(int page)
        {
            Paging paging = new Paging(page, 5);
            string url = RequestContext.RouteData.Values["url"] as string;
            if (string.IsNullOrEmpty(url))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid URL");
            }
            PagedList<Review> reviews = await ReviewService.GetReviewsAsync(url, paging);
            PagedList<ReviewDetails> reviewDetails = new PagedList<ReviewDetails>(new List<ReviewDetails>(), 0,0);
            reviewDetails.TotalCount = reviews.TotalCount;
            reviewDetails.PageSize = reviews.PageSize;
            reviewDetails.PageCount = reviews.PageCount;


            reviewDetails.List = reviews.List
                    .Select(x => Mapper.Map<ReviewDetails>(x))
                    .ToList();
            if (reviews == null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to get reviews");
            }

            float averageRating = 0;
            foreach (Review review in reviews.List)
            {
                if (review.ReviewScore != 0)
                {
                    averageRating += review.ReviewScore;
                }
                reviewDetails.AverageRating = averageRating / reviews.List.Count(x => x.ReviewScore != 0);
                reviewDetails.AverageRating = (float)Math.Round(reviewDetails.AverageRating, 2);
            }
            return Request.CreateResponse(HttpStatusCode.OK, reviewDetails);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Business, User")]
        [Route("Api/Review/{url}")]
        public async Task<HttpResponseMessage> UpdateReviewAsync(UpdateReview createReview)
        {
            string url = RequestContext.RouteData.Values["url"] as string;
            if (string.IsNullOrEmpty(url))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid URL");
            }
            Review review = Mapper.Map<Review>(createReview);
            review.Website = new Website { URL = url };
            
            switch (await ReviewService.UpdateReviewAsync(review))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to edit a review");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.OK, "Review edited succesfully");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Business, User")]
        [Route("Api/Review")]
        public async Task<HttpResponseMessage> DeleteReviewAsync(Guid id)
        {
            switch (await ReviewService.DeleteReviewAsync(id))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to delete a review");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.OK, "Review deleted succesfully");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        [Route("Api/Review/{url}")]
        public async Task<HttpResponseMessage> AddReviewAsync(CreateReview createReview)
        {
            string url = RequestContext.RouteData.Values["url"] as string;
            if (string.IsNullOrEmpty(url))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid URL");
            }
            Review review = Mapper.Map<Review>(createReview);
            review.Website = new Website { URL = url };
            Review reviewToReturn = await ReviewService.CreateReviewAsync(review);
            if (reviewToReturn == null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to add a review");
            }
            return Request.CreateResponse(HttpStatusCode.Created, "Review added succesfully");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Business")]
        [Route("Api/Reply/{url}")]
        public async Task<HttpResponseMessage> AddReplyAsync(CreateReply createReply)
        {
            string url = RequestContext.RouteData.Values["url"] as string;
            if (string.IsNullOrEmpty(url))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid URL");
            }
            Review review = Mapper.Map<Review>(createReply);
            review.Website = new Website { URL = url };
            int reviewToReturn = await ReviewService.CreateReplyAsync(review);
            if (reviewToReturn == 0)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to add a reply");
            }
            return Request.CreateResponse(HttpStatusCode.Created, "Reply added succesfully");
        }
    }
}