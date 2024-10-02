using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Responses.Flashcards;

public class FlashcardExamGradeResponse
{
    public int FlashcardExamGradeId { get; set; }

    public string QuestionTitle { get; set; } = string.Empty;
    
    public string? QuestionDesc { get; set; } = null!;

    public int QuestionNumber { get; set; }

    public string CorrectAnswer { get; set; } = null!;
    
    public string UserAnswer { get; set; } = null!;
    
    public string FlashcardGradeStatus { get; set; } = null!;
    
    public int FlashcardExamHistoryId { get; set; }
    
    public string? QuestionType { get; set; } = string.Empty;
    
    public int FlashcardDetailId { get; set; }

    public FlashcardDetailDto FlashcardDetail { get; set; } = null!;
}