using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class Flashcard
{
    public int FlashcardId { get; set; }

    public string Title { get; set; } = null!;

    public int? TotalWords { get; set; }

    public int? TotalView { get; set; }

    public string? Description { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool IsPublic { get; set; }

    public virtual ICollection<FlashcardDetail> FlashcardDetails { get; set; } = new List<FlashcardDetail>();

    public virtual ICollection<UserFlashcard> UserFlashcards { get; set; } = new List<UserFlashcard>();
}
