using iskustvohr.Model;
using iskustvohr.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iskustvohr.WebApi.Models
{
    public class ReviewDetails
    {
        public Guid Id { get; set; }
        public UserDetails User { get; set; }
        public string ReviewText { get; set; }
        public int ReviewScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ReplyDetails Reply { get; set; }
    }
}