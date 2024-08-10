using iskustvohr.Model;
using iskustvohr.Repository.Common;
using iskustvohr.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace iskustvohr.Service
{
    public class WebsiteService : IWebsiteService
    {
        protected IWebsiteRepository WebsiteRepository { get; set; }
        protected HttpClient HttpClient { get; set; }

        public WebsiteService(IWebsiteRepository websiteRepository, HttpClient httpClient)
        {
            WebsiteRepository = websiteRepository;
            HttpClient = httpClient;
        }

        public async Task<Website> GetWebsiteAsync(Website website)
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync("https://" + website.URL);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return null;
            }
            
            Website websiteToReturn = await WebsiteRepository.GetWebsiteByUrlAsync(website);
            if(websiteToReturn == null)
            {
                websiteToReturn = await CreateWebsiteAsync(website);
            }
            return websiteToReturn;
        }

        private async Task<Website> CreateWebsiteAsync(Website website)
        {
            HttpResponseMessage responsePhoto = await HttpClient.GetAsync("https://t2.gstatic.com/faviconV2?client=SOCIAL&type=FAVICON&fallback_opts=TYPE,SIZE,URL&url=https://" + website.URL + "&size=256");
            HttpResponseMessage responseName = await HttpClient.GetAsync("https://websitemetagetter.vercel.app/website/" + website.URL + "?_rsc=zxefk");

            string websiteTitle = responseName.Content.ReadAsStringAsync().Result;
            string title = Regex.Match(websiteTitle, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>",
    RegexOptions.IgnoreCase).Groups["Title"].Value;
            title = title.Substring(0, title.IndexOf("| Website Meta Getter"));
            if(title.Contains(" Metadata"))
                title = title.Substring(0, title.IndexOf(" Metadata"));
            string photoUrl = responsePhoto.RequestMessage.RequestUri.ToString();
            Website newWebsite = new Website
            {
                Id = Guid.NewGuid(),
                Name = title,
                PhotoUrl = photoUrl,
                URL = website.URL,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsAssigned = false,
                IsActive = true
            };
            return await WebsiteRepository.CreateWebsite(newWebsite);
        }
    }
}
