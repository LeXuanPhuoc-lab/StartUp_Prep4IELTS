namespace Prep4IELTS.Data.Entities;

public partial class FlashcardExamGrade
{
    public int FlashcardExamGradeId { get; set; }
    
    public string Answer { get; set; } = null!;
    
    public string FlashcardGradeStatus { get; set; } = null!;
    
    public int FlashcardExamHistoryId { get; set; }
    
    public int FlashcardDetailId { get; set; }

    public FlashcardExamHistory FlashcardExamHistory { get; set; } = null!;

    public FlashcardDetail FlashcardDetail { get; set; } = null!;
}