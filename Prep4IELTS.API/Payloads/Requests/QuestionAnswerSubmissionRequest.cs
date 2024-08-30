namespace EXE202_Prep4IELTS.Payloads.Requests;

public class QuestionAnswerSubmissionRequest
{
    public int QuestionId { get; set; }
    public string SelectedAnswer { get; set; } = string.Empty;
}