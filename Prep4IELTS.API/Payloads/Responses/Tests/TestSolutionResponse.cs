namespace EXE202_Prep4IELTS.Payloads.Responses.Tests;

public class TestSolutionResponse
{
    public string TestTitle { get; set; } = string.Empty;
    public List<SectionSolutionResponse> Sections { get; set; } = new();
}