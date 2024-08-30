namespace Prep4IELTS.Business.Models;

public class QuestionAnswerSubmissionModel
{
    public int QuestionId { get; set; }
    public string SelectedAnswer { get; set; } = string.Empty;
}