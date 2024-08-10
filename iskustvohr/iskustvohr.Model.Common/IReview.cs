using System;

namespace iskustvohr.Model.Common
{
    public interface IReview
    {
        DateTime CreatedAt { get; set; }
        Guid Id { get; set; }
        bool IsActive { get; set; }
        int ReviewScore { get; set; }
        string ReviewText { get; set; }
        DateTime UpdatedAt { get; set; }
        IUser User { get; set; }
        IWebsite Website { get; set; }
        IReply Reply { get; set; }
        bool IsReview { get; set; }
    }
}