using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iskustvohr.WebApi.Models
{
    public class UpdateReview
    {
        public Guid Id { get; set; }
        public string ReviewText { get; set; }
        public int ReviewScore { get; set; }
    }
}