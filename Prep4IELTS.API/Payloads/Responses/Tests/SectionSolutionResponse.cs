namespace EXE202_Prep4IELTS.Payloads.Responses.Tests;

public class SectionSolutionResponse
{
    public string SectionName { get; set; } = string.Empty;
    public string Transcript { get; set; } = string.Empty;
    public List<QuestionAnswerDisplayResponse> QuestionAnswers { get; set; } = new();
}