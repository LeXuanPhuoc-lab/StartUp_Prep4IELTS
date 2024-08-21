using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class PartitionTag
{
    public int PartitionTagId { get; set; }

    public string? PartitionTagDesc { get; set; }

    public virtual ICollection<TestSectionPartition> TestSectionPartitions { get; set; } = new List<TestSectionPartition>();
}
