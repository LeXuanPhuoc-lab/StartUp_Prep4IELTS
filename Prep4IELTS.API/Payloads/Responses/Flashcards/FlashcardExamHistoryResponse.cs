using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Responses.Flashcards;

public class FlashcardExamHistoryResponse
{
    public int FlashcardExamHistoryId { get; set; }

    public int? TotalQuestion { get; set; }
    
    public int? TotalRightAnswer { get; set; }
    
    public int? TotalWrongAnswer { get; set; }
    
    public int? TotalCompletionTime { get; set; }
    
    public double? AccuracyRate { get; set; }
    
    public DateTime TakenDate { get; set; }

    public int UserFlashcardId { get; set; }

    public List<FlashcardExamGradeResponse> FlashcardExamGrades { get; set; } = new();
}

public static class FlashcardExamHistoryResponseExtensions
{
    public static FlashcardExamHistoryResponse ToFlashcardExamHistoryResponse(
        this FlashcardExamHistoryDto flashcardExamHisDto)
    {
        // Determine whether is term pattern question or not 
        var isTermPattern = flashcardExamHisDto.IsTermPattern;
        
        // Initiate flashcard exam grade list 
        List<FlashcardExamGradeResponse> flashcardExamGrades = new();
        foreach (var feg in flashcardExamHisDto.FlashcardExamGrades)
        {
            flashcardExamGrades.Add(new()
            {
                FlashcardExamGradeId = feg.FlashcardExamGradeId,
                FlashcardExamHistoryId = feg.FlashcardExamHistoryId,
                UserAnswer = feg.Answer,
                CorrectAnswer = isTermPattern 
                    ? feg.FlashcardDetail.Definition 
                    : feg.FlashcardDetail.WordText,
                QuestionNumber = feg.QuestionNumber,
                QuestionType = feg.QuestionType,
                QuestionTitle = isTermPattern 
                    ? feg.FlashcardDetail.WordText 
                    : feg.FlashcardDetail.Definition,
                QuestionDesc = feg.QuestionDesc,
                FlashcardGradeStatus = feg.FlashcardGradeStatus,
                FlashcardDetailId = feg.FlashcardDetailId,
                FlashcardDetail = feg.FlashcardDetail
            });
        }
        
        // Initiate flashcard exam history
        return new()
        {
            FlashcardExamHistoryId = flashcardExamHisDto.FlashcardExamHistoryId,
            TotalQuestion = flashcardExamHisDto.TotalQuestion,
            TotalRightAnswer = flashcardExamHisDto.TotalRightAnswer,
            TotalWrongAnswer = flashcardExamHisDto.TotalWrongAnswer,
            TotalCompletionTime = flashcardExamHisDto.TotalCompletionTime,
            AccuracyRate = flashcardExamHisDto.AccuracyRate,
            TakenDate = flashcardExamHisDto.TakenDate,
            UserFlashcardId = flashcardExamHisDto.UserFlashcardId,
            FlashcardExamGrades = flashcardExamGrades
        };
    }
}