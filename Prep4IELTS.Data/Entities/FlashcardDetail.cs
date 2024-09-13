using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class FlashcardDetail
{
    public int FlashcardDetailId { get; set; }

    public string WordText { get; set; } = null!;

    public string Definition { get; set; } = null!;

    public string WordForm { get; set; } = null!;

    public string? WordPronunciation { get; set; }

    public string? Example { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public int FlashcardId { get; set; }

    public virtual Flashcard Flashcard { get; set; } = null!;

    public virtual ICollection<UserFlashcardProgress> UserFlashcardProgresses { get; set; } = new List<UserFlashcardProgress>();
}
