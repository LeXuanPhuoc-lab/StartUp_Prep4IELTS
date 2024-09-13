using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class QuestionAnswer
{
    public int QuestionAnswerId { get; set; }

    public string AnswerDisplay { get; set; } = null!;

    public string AnswerText { get; set; } = null!;

    public bool IsTrue { get; set; }

    public int QuestionId { get; set; }

    public virtual Question Question { get; set; } = null!;
}
