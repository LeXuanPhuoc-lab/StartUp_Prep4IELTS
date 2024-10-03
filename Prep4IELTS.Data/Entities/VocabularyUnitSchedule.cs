namespace Prep4IELTS.Data.Entities;

public partial class VocabularyUnitSchedule
{
    public int VocabularyUnitScheduleId { get; set; }

    public string? Weekday { get; set; } 

    public string? Comment { get; set; } 

    public DateTime CreateDate { get; set; }

    public int FlashcardDetailId { get; set; }

    
    public int UserFlashcardId { get; set; }
    
    public virtual FlashcardDetail FlashcardDetail { get; set; } = null!;

    public virtual UserFlashcard UserFlashcard { get; set; } = null!;
}