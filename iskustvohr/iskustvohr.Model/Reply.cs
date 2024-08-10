using iskustvohr.Model.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iskustvohr.Model
{
    public class Reply : IReply
    {
        public Guid Id { get; set; }
        public IUser User { get; set; }
        public string ReplyText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
