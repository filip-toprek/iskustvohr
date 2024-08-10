using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iskustvohr.WebApi.Models
{
    public class CreateReply
    {
        public Guid ReviewId { get; set; }
        public string ReplyText { get; set; }
    }
}