using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class UserFlashcard
{
    public int UserFlashcardId { get; set; }

    public Guid UserId { get; set; }

    public int FlashcardId { get; set; }

    public virtual Flashcard Flashcard { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserFlashcardProgress> UserFlashcardProgresses { get; set; } = new List<UserFlashcardProgress>();
}
