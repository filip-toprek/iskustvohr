using System;

namespace iskustvohr.Model.Common
{
    public interface IWebsite
    {
        DateTime CreatedAt { get; set; }
        Guid Id { get; set; }
        string PhotoUrl { get; set; }
        bool IsActive { get; set; }
        string Name { get; set; }
        DateTime UpdatedAt { get; set; }
        bool IsAssigned { get; set; }
        string URL { get; set; }
    }
}