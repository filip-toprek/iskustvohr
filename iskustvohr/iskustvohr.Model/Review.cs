using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iskustvohr.Model.Common;

namespace iskustvohr.Model
{
    public class Review : IReview
    {
        public Guid Id { get; set; }
        public IUser User { get; set; }
        public IWebsite Website { get; set; }
        public string ReviewText { get; set; }
        public int ReviewScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public IReply Reply { get; set; }
        public bool IsReview { get; set; }
    }
}
