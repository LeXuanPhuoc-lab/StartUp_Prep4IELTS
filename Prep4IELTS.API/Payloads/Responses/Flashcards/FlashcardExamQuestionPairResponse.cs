namespace EXE202_Prep4IELTS.Payloads.Responses.Flashcards;

public class FlashcardExamQuestionPairResponse
{
    public FlashcardExamQuestionResponse Question { get; set; } = null!;
    public FlashcardExamQuestionAnswerResponse Answer { get; set; } = null!;
}