using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public partial class SpeakingSample
{
    public int SpeakingSampleId { get; set; }

    public string SpeakingSampleName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateDate { get; set; }

    // public int TopicId { get; set; }

    public virtual ICollection<SpeakingPart> SpeakingParts { get; set; } = new List<SpeakingPart>();

    // public virtual SpeakingTopic Topic { get; set; } = null!;
    
    [JsonIgnore]
    public virtual ICollection<UserSpeakingSampleHistory> UserSpeakingSampleHistories { get; set; } = new List<UserSpeakingSampleHistory>();
}
