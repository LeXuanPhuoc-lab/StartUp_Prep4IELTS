using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class UserSpeakingSampleHistory
{
    public int UserSampleHistoryId { get; set; }

    public Guid UserId { get; set; }

    public int SpeakingSampleId { get; set; }
    
    public virtual SpeakingSample SpeakingSample { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
