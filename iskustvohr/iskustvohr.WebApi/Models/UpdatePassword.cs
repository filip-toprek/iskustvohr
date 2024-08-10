using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iskustvohr.WebApi.Models
{
    public class UpdatePassword
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public string PasswordResetToken { get; set; }
    }
}