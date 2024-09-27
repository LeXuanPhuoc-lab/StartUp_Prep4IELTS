namespace Prep4IELTS.Data.Dtos;

public record FlashcardExamGradeDto(
    int FlashcardExamGradeId, 
    string Answer, 
    string FlashcardGradeStatus, 
    int FlashcardExamHistoryId,
    int FlashcardDetailId, 
    FlashcardExamHistoryDto FlashcardExamHistory, 
    FlashcardDetailDto FlashcardDetail);