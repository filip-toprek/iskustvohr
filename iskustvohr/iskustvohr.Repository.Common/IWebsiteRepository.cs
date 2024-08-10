using iskustvohr.Model;
using System.Threading.Tasks;

namespace iskustvohr.Repository.Common
{
    public interface IWebsiteRepository
    {
        Task<Website> CreateWebsite(Website website);
        Task<Website> GetWebsiteByUrlAsync(Website website);
    }
}