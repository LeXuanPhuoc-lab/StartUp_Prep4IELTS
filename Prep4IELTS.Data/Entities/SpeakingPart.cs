using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class SpeakingPart
{
    public int PartId { get; set; }

    public int PartNumber { get; set; }

    public string? PartDescription { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateDate { get; set; }

    public int TopicSampleId { get; set; }

    public virtual SpeakingTopicSample TopicSample { get; set; } = null!;
}
