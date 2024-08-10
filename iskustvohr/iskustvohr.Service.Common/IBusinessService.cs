using iskustvohr.Model;
using System.Threading.Tasks;

namespace iskustvohr.Service.Common
{
    public interface IBusinessService
    {
        Task<int> CreateBusinessAsync(Business newBusiness);
        Task<int> VerifyBusinessAsync(Business newBusiness);
    }
}