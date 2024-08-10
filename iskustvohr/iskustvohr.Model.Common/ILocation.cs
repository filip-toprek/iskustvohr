using System;

namespace iskustvohr.Model.Common
{
    public interface ILocation
    {
        string City { get; set; }
        Guid Id { get; set; }
    }
}