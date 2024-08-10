using AutoMapper;
using iskustvohr.Model;
using iskustvohr.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iskustvohr.WebApi.MapperModels
{
    public class WebsiteProfile : Profile
    {
        public WebsiteProfile()
        {
            CreateMap<Website, WebsiteDetails>();
        }
    }
}