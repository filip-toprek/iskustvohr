using iskustvohr.Model.Common;
using System;

namespace iskustvohr.Model.Common
{
    public interface IReply
    {
        DateTime CreatedAt { get; set; }
        Guid CreatedBy { get; set; }
        Guid Id { get; set; }
        bool IsActive { get; set; }
        string ReplyText { get; set; }
        DateTime UpdatedAt { get; set; }
        Guid UpdatedBy { get; set; }
        IUser User { get; set; }
    }
}