using iskustvohr.Model;
using System.Threading.Tasks;

namespace iskustvohr.Service.Common
{
    public interface IUserService
    {
        Task<User> LoginUserAsync(User user);
        Task<User> RegisterUserAsync(User user);
        Task<User> GetUserByIdAsync(User user);
        Task<int> UpdateUserProfileAsync(User user);
        Task<int> ConfirmEmailAsync(User user);
        Task<int> RequestResetPasswordAsync(User user);
        Task<int> ChangePasswordAsync(User user);
    }
}