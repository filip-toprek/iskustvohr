using iskustvohr.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iskustvohr.Model
{
    public class Comment : IComment
    {
        public Guid Id { get; set; }
        public IReview Review { get; set; }
        public IUser User { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
