using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public partial class TestGrade
{
    public int TestGradeId { get; set; }

    public string GradeStatus { get; set; } = null!;

    public int QuestionNumber { get; set; }

    public string RightAnswer { get; set; } = null!;

    public string InputedAnswer { get; set; } = null!;

    public int QuestionId { get; set; }

    public int PartitionHistoryId { get; set; }

    [JsonIgnore]
    public virtual PartitionHistory PartitionHistory { get; set; } = null!;

    [JsonIgnore]
    public virtual Question Question { get; set; } = null!;
}
