using System;

namespace iskustvohr.Model.Common
{
    public interface IComment
    {
        DateTime CreatedAt { get; set; }
        Guid Id { get; set; }
        bool IsActive { get; set; }
        IReview Review { get; set; }
        string Text { get; set; }
        DateTime UpdatedAt { get; set; }
        IUser User { get; set; }
    }
}