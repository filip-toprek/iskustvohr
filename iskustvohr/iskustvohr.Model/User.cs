using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iskustvohr.Model.Common;

namespace iskustvohr.Model
{
    public class User : IUser
    {
        public User(){}

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfileImageUrl { get; set; }
        public IRole Role { get; set; }
        public IBusiness Business { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool EmailConfirmed { get; set; }
        public Guid EmailVerificationId { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }
        public bool IsActive { get; set; }
    }
}
