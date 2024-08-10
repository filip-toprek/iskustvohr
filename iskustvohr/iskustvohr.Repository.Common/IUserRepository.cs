using iskustvohr.Model;
using System;
using System.Threading.Tasks;

namespace iskustvohr.Repository.Common
{
    public interface IUserRepository
    {
        Task<User> CreateNewUserAsync(User user);
        Task<User> GetUserByEmailAsync(User user);
        Task<User> GetUserByIdAsync(User user);
        Task<int> UpdateUserPasswordResetAsync(User user);
        Task<int> UpdateUserPasswordAsync(User user);
        Task<int> UpdateUserProfileAsync(User user);
        Task<int> ConfirmEmailAsync(User user);
        Task<int> UpdateUserProfilePhotoAsync(User user);
    }
}