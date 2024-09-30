namespace EXE202_Prep4IELTS.Payloads.Requests.Flashcards;

public class FlashcardExamQuestionAnswerRequest
{
    public int QuestionNumber { get; set; }
    public string QuestionType { get; set; } = null!;
    public int FlashcardDetailId { get; set; }
    public string? Answer { get; set; }
}