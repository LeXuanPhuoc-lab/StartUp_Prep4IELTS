using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class SpeakingTopic
{
    public int TopicId { get; set; }

    public string TopicName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ICollection<SpeakingTopicSample> SpeakingTopicSamples { get; set; } = new List<SpeakingTopicSample>();
}
