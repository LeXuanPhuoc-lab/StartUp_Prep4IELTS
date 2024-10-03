using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public partial class UserFlashcard
{
    public int UserFlashcardId { get; set; }

    public Guid UserId { get; set; }

    public int FlashcardId { get; set; }
    
    public virtual Flashcard Flashcard { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserFlashcardProgress> UserFlashcardProgresses { get; set; } = new List<UserFlashcardProgress>();
    
    [JsonIgnore]
    public virtual ICollection<FlashcardExamHistory> FlashcardExamHistories { get; set; } = new List<FlashcardExamHistory>();
    
    [JsonIgnore]
    public virtual ICollection<VocabularyUnitSchedule> VocabularyUnitSchedules { get; set; } = new List<VocabularyUnitSchedule>();
}
