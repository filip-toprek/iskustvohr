using iskustvohr.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iskustvohr.WebApi.Models
{
    public class CreateReview
    {
        public string ReviewText { get; set; }
        public int ReviewScore { get; set; }
    }
}