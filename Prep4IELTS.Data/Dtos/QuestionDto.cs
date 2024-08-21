using System.Text.Json.Serialization;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Dtos;

public record QuestionDto(
    int QuestionId,
    string? QuestionDesc,
    string? QuestionAnswerExplanation,
    int QuestionNumber,
    bool IsMultipleChoice,
    ICollection<QuestionAnswerDto> QuestionAnswers)
{
    public int TestSectionPartId { get; set; }
    [JsonIgnore] public TestSectionPartition TestSectionPart { get; set; } = null!;
    [JsonIgnore] public ICollection<TestGradeDto> TestGrades { get; set; } = new List<TestGradeDto>();
};