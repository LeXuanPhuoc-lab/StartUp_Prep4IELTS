﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public partial class PartitionHistory
{
    public int PartitionHistoryId { get; set; }

    public string TestSectionName { get; set; } = null!;

    public int? TotalRightAnswer { get; set; }

    public int? TotalWrongAnswer { get; set; }

    public int? TotalSkipAnswer { get; set; }

    public int TotalQuestion { get; set; }

    public int TestHistoryId { get; set; }

    public int? TestSectionPartId { get; set; }

    public virtual ICollection<TestGrade> TestGrades { get; set; } = new List<TestGrade>();

    [JsonIgnore]
    public virtual TestHistory TestHistory { get; set; } = null!;
    
    [JsonIgnore]
    public virtual TestSectionPartition TestSectionPart { get; set; } = null!;
}
