using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Requests.Flashcards;

public class FlashcardExamSubmissionRequest
{
    public DateTime TakenDate { get; set; }
    public int TotalCompletionTime { get; set; }
    public int FlashcardId { get; set; }
    public bool IsTermPattern { get; set; }
    public ICollection<FlashcardExamQuestionAnswerRequest> QuestionAnswers { get; set; } 
        = new List<FlashcardExamQuestionAnswerRequest>();
}

public static class FlashcardExamSubmissionRequestExtensions
{
    public static FlashcardExamHistoryDto ToFlashcardExamHistoryDto(
        this FlashcardExamSubmissionRequest request,
        int userFlashcardId)
    {
        // Initiate exam grades for flashcard exam history
        List<FlashcardExamGradeDto> flashcardExamGrades = new();
        foreach (var eg in request.QuestionAnswers)
        {
            flashcardExamGrades.Add(new(
                FlashcardExamGradeId: 0,
                QuestionNumber: eg.QuestionNumber,
                Answer: eg.Answer ?? null!,
                FlashcardGradeStatus: string.Empty,
                FlashcardExamHistoryId: 0,
                FlashcardDetailId: eg.FlashcardDetailId,
                QuestionType: eg.QuestionType,
                FlashcardExamHistory: null!,
                FlashcardDetail: null!
                ));
        }

        return new FlashcardExamHistoryDto(
            FlashcardExamHistoryId: 0,
            TotalQuestion: flashcardExamGrades.Count,
            TotalRightAnswer: 0,
            TotalWrongAnswer: 0,
            TotalCompletionTime: request.TotalCompletionTime,
            AccuracyRate: 0,
            TakenDate: request.TakenDate,
            UserFlashcardId: userFlashcardId,
            FlashcardExamGrades: flashcardExamGrades);
    }
}