using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class TestSectionPartition
{
    public int TestSectionPartId { get; set; }

    public string? PartitionDesc { get; set; }

    public bool IsVerticalLayout { get; set; }

    public string? PartitionImage { get; set; }

    public int TestSectionId { get; set; }

    public int PartitionTagId { get; set; }

    public virtual ICollection<PartitionHistory> PartitionHistories { get; set; } = new List<PartitionHistory>();

    public virtual PartitionTag PartitionTag { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual TestSection TestSection { get; set; } = null!;
}
