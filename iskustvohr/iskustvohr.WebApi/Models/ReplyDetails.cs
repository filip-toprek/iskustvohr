using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iskustvohr.WebApi.Models
{
    public class ReplyDetails
    {
        public Guid Id { get; set; }
        public UserDetails User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ReplyText { get; set; }
    }
}