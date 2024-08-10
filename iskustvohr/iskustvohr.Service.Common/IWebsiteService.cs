using iskustvohr.Model;
using System.Threading.Tasks;

namespace iskustvohr.Service.Common
{
    public interface IWebsiteService
    {
        Task<Website> GetWebsiteAsync(Website website);
    }
}