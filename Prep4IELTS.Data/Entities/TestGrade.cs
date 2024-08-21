using System;
using System.Collections.Generic;

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

    public virtual PartitionHistory PartitionHistory { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;
}
