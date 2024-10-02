namespace Prep4IELTS.Data.Dtos;

public record FlashcardExamGradeDto(
    int FlashcardExamGradeId,
    string? QuestionDesc,
    int QuestionNumber,
    string Answer, 
    string FlashcardGradeStatus, 
    int FlashcardExamHistoryId,
    int FlashcardDetailId, 
    string? QuestionType,
    FlashcardExamHistoryDto FlashcardExamHistory, 
    FlashcardDetailDto FlashcardDetail);