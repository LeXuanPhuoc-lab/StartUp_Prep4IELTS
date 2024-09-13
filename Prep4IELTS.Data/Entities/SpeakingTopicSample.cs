using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class SpeakingTopicSample
{
    public int TopicSampleId { get; set; }

    public string TopicSampleName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateDate { get; set; }

    public int TopicId { get; set; }

    public virtual ICollection<SpeakingPart> SpeakingParts { get; set; } = new List<SpeakingPart>();

    public virtual SpeakingTopic Topic { get; set; } = null!;

    public virtual ICollection<UserSpeakingSampleHistory> UserSpeakingSampleHistories { get; set; } = new List<UserSpeakingSampleHistory>();
}
