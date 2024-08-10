using AutoMapper;
using iskustvohr.Model;
using iskustvohr.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iskustvohr.WebApi.MapperModels
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDetails>();
            CreateMap<Review, UserReviewDetails>()
                .ForMember(dest => dest.WebsiteUrl, opt => opt.MapFrom(src => src.Website.URL))
                .ForMember(dest => dest.WebsitePhoto, opt => opt.MapFrom(src => src.Website.PhotoUrl));
            CreateMap<CreateReview, Review>();
            CreateMap<UpdateReview, Review>();
            CreateMap<Reply, ReplyDetails>();
            CreateMap<CreateReply, Review>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ReviewId))
                .ForMember(dest => dest.ReviewText, opt => opt.MapFrom(src => src.ReplyText));
        }
    }
}