using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using AutoMapper;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using iskustvohr.Service;
using iskustvohr.Service.Common;
using iskustvohr.Repository;
using iskustvohr.Repository.Common;
using iskustvohr.WebApi.Authorization;
using Microsoft.Owin.Security.OAuth;
using System.Net.Http;

namespace iskustvohr.WebApi.App_Start
{
    public class DIConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<BusinessService>().As<IBusinessService>();
            builder.RegisterType<WebsiteService>().As<IWebsiteService>();
            builder.RegisterType<ReviewService>().As<IReviewService>();
            builder.RegisterType<WebsiteRepository>().As<IWebsiteRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<ReviewRepository>().As<IReviewRepository>();
            builder.RegisterType<BusinessRepository>().As<IBusinessRepository>();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>();
            builder.Register(c => new HttpClient()).As<HttpClient>();
            builder.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}