using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iskustvohr.WebApi.Models
{
    public class UserDetails
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}