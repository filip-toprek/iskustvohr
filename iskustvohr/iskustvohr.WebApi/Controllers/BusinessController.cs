using AutoMapper;
using iskustvohr.Model;
using iskustvohr.Service;
using iskustvohr.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using iskustvohr.Service.Common;

namespace iskustvohr.WebApi.Controllers
{
    public class BusinessController : ApiController
    {
        protected IBusinessService BusinessService { get; set; }
        public BusinessController(IBusinessService businessService)
        {
            BusinessService = businessService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        [Route("Api/Business/{url}")]
        public async Task<HttpResponseMessage> ApplyForBusinessAsync(CreateBusiness createBusiness)
        {
            string url = RequestContext.RouteData.Values["url"] as string;
            if (string.IsNullOrEmpty(url))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid URL");
            }
            Business newBusiness = new Business
            {
                BusinessEmail = createBusiness.BusinessEmail,
                Website = new Website { URL = url }
            };
            switch (await BusinessService.CreateBusinessAsync(newBusiness))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to add a business");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.Created, "Business added succesfully");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin, User")]
        [Route("Api/Business/{url}/{verificationId}")]
        public async Task<HttpResponseMessage> VerifyBusinessAsync()
        {
            string url = RequestContext.RouteData.Values["url"] as string;
            if (string.IsNullOrEmpty(url))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid URL");
            }

            string verificationId = RequestContext.RouteData.Values["verificationId"] as string;
            if (string.IsNullOrEmpty(url))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid or expired verification link.");
            }

            Business newBusiness = new Business
            {
                Website = new Website { URL = url },
                EmailVerificationId = Guid.Parse(verificationId)
            };
            switch (await BusinessService.VerifyBusinessAsync(newBusiness))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to verify a business");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.Created, "Business verified succesfully");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error");
            }
        }
    }
}