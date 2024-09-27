namespace Prep4IELTS.Data.Entities;

public partial class VocabularyUnitSchedule
{
    public int VocabularyUnitScheduleId { get; set; }

    public string Weekday { get; set; } = null!;

    public string Comment { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public int FlashcardDetailId { get; set; }

    public virtual FlashcardDetail FlashcardDetail { get; set; } = null!;
}