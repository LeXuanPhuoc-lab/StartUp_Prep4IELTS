using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public partial class TestCategory
{
    public int TestCategoryId { get; set; }

    public string? TestCategoryName { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();
    
    [JsonIgnore]
    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
