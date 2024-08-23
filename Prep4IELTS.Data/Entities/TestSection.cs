﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public partial class TestSection
{
    public int TestSectionId { get; set; }

    public string TestSectionName { get; set; } = null!;

    public string? ReadingDesc { get; set; }

    public string? AudioResourceUrl { get; set; }

    public int TotalQuestion { get; set; }

    public string? SectionTranscript { get; set; }

    public Guid TestId { get; set; }

    [JsonIgnore]
    public virtual Test Test { get; set; } = null!;
    
    public virtual ICollection<TestSectionPartition> TestSectionPartitions { get; set; } = new List<TestSectionPartition>();
}
