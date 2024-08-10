using iskustvohr.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iskustvohr.Model
{
    public class Business : IBusiness
    {
        public Guid Id { get; set; }
        public IWebsite Website { get; set; }
        public bool IsConfirmed { get; set; }
        public Guid EmailVerificationId { get; set; }
        public string BusinessEmail { get; set; }
    }
}
