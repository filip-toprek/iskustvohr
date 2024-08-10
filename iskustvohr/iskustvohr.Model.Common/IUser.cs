using System;

namespace iskustvohr.Model.Common
{
    public interface IUser
    {
        DateTime CreatedAt { get; set; }
        string FirstName { get; set; }
        Guid Id { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string ProfileImageUrl { get; set; }
        bool IsActive { get; set; }
        string LastName { get; set; }
        IRole Role { get; set; }
        DateTime UpdatedAt { get; set; }
        IBusiness Business { get; set; }
    }
}