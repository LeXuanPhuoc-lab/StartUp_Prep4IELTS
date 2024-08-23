using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public partial class SystemRole
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
