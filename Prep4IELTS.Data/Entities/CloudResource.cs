using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class CloudResource
{
    public int CloudResourceId { get; set; }

    public string? PublicId { get; set; }

    public string Url { get; set; } = null!;

    public int? Bytes { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<TestSectionPartition> TestSectionPartitions { get; set; } = new List<TestSectionPartition>();

    public virtual ICollection<TestSection> TestSections { get; set; } = new List<TestSection>();
}
