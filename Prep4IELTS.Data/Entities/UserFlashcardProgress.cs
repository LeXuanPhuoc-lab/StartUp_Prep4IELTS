using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class UserFlashcardProgress
{
    public int UserFlashcardProgressId { get; set; }

    public string ProgressStatus { get; set; } = null!;

    public int UserFlashcardId { get; set; }

    public int FlashcardDetailId { get; set; }

    public virtual FlashcardDetail FlashcardDetail { get; set; } = null!;

    public virtual UserFlashcard UserFlashcard { get; set; } = null!;
}
