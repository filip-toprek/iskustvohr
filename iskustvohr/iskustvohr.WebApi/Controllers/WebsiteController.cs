using AutoMapper;
using iskustvohr.Model;
using iskustvohr.Service.Common;
using iskustvohr.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace iskustvohr.WebApi.Controllers
{
    public class WebsiteController : ApiController
    {
        protected IWebsiteService WebsiteService { get; set; }
        protected IMapper Mapper { get; set; }
        public WebsiteController(IWebsiteService websiteService, IMapper mapper)
        {
            WebsiteService = websiteService;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("Api/Website/{url}")]
        public async Task<HttpResponseMessage> GetWebsiteAsync()
        {
            string url = RequestContext.RouteData.Values["url"] as string;
            if (string.IsNullOrEmpty(url))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid URL");
            }

            Website website = await WebsiteService.GetWebsiteAsync(new Website { URL = url });
            if(website == null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to get website");
            }

            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<WebsiteDetails>(website));
        }
    }
}