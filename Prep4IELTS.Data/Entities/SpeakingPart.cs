using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public partial class SpeakingPart
{
    public int SpeakingPartId { get; set; }

    public int SpeakingPartNumber { get; set; }

    public string? SpeakingPartDescription { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateDate { get; set; }

    public int SpeakingSampleId { get; set; }
    
    [JsonIgnore]
    public virtual SpeakingSample SpeakingSample { get; set; } = null!;
}
