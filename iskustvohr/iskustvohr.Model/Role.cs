using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iskustvohr.Model.Common;

namespace iskustvohr.Model
{
    public class Role : IRole
    {
        public Role(){}
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }
}
