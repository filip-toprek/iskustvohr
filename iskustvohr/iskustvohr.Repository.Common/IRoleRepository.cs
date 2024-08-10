using iskustvohr.Model;
using System.Threading.Tasks;

namespace iskustvohr.Repository.Common
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleByNameAsync(Role role);
        Task<Role> GetRoleByIdAsync(Role role);
    }
}