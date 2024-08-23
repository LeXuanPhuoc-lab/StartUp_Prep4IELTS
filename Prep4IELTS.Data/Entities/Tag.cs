using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public partial class Tag
{
    public int TagId { get; set; }

    public string? TagName { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
