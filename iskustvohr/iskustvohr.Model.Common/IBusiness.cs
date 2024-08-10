using iskustvohr.Model.Common;
using System;

namespace iskustvohr.Model.Common
{
    public interface IBusiness
    {
        Guid Id { get; set; }
        bool IsConfirmed { get; set; }
        IWebsite Website { get; set; }
    }
}