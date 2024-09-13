using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class SystemRole
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
