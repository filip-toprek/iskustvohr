using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iskustvohr.WebApi.Models
{
    public class WebsiteDetails
    {
        public string URL { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsActive { get; set; }
    }
}