using iskustvohr.Model;
using System;
using System.Threading.Tasks;

namespace iskustvohr.Repository.Common
{
    public interface IBusinessRepository
    {
        Task<int> CreateBusinessAsync(Business newBusiness);
        Task<int> VerifyBusinessAsync(Business newBusiness, Role role, Guid userId);
        Task<Business> GetBusinessByWebsiteIdAsync(Website website);
    }
}