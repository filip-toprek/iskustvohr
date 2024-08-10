using AutoMapper;
using iskustvohr.Model;
using iskustvohr.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iskustvohr.WebApi.MapperModels
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDetails>();
            CreateMap<UpdatePassword, User>();
            CreateMap<UpdateUser, User>();
            CreateMap<RegisterUser, User>();
        }
    }
}