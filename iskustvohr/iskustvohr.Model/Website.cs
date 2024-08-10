using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iskustvohr.Model.Common;

namespace iskustvohr.Model
{
    public class Website : IWebsite
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string URL { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsActive { get; set; }
    }
}
