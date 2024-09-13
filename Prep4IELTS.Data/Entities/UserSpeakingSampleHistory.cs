using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class UserSpeakingSampleHistory
{
    public int UserSampleHistoryId { get; set; }

    public Guid UserId { get; set; }

    public int TopicSampleId { get; set; }

    public virtual SpeakingTopicSample TopicSample { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
