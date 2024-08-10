using System;

namespace iskustvohr.Model.Common
{
    public interface IRole
    {
        Guid Id { get; set; }
        bool IsActive { get; set; }
        string RoleName { get; set; }
    }
}