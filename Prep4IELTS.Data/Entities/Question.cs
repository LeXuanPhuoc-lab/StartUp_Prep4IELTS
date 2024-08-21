using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class Question
{
    public int QuestionId { get; set; }

    public string? QuestionDesc { get; set; }

    public string? QuestionAnswerExplanation { get; set; }

    public int QuestionNumber { get; set; }

    public bool IsMultipleChoice { get; set; }

    public int TestSectionPartId { get; set; }

    public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();

    public virtual ICollection<TestGrade> TestGrades { get; set; } = new List<TestGrade>();

    public virtual TestSectionPartition TestSectionPart { get; set; } = null!;
}
