using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public partial class Test
{
    public int Id { get; set; }

    public Guid TestId { get; set; }

    public string TestTitle { get; set; } = null!;

    public int Duration { get; set; }

    public string TestType { get; set; } = null!;

    public int? TotalEngaged { get; set; }

    public int TotalQuestion { get; set; }

    public int? TotalSection { get; set; }

    public int TestCategoryId { get; set; }
    
    public DateTime CreateDate { get; set; }
    
    public DateTime? ModifiedDate { get; set; }

    public string? CreateBy { get; set; } = string.Empty;
    
    [JsonIgnore]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual TestCategory TestCategory { get; set; } = null!;
    
    // [JsonIgnore]
    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();
    
    [JsonIgnore]
    public virtual ICollection<TestSection> TestSections { get; set; } = new List<TestSection>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
