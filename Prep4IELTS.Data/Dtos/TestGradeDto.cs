using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record TestGradeDto(
    int TestGradeId,
    string GradeStatus,
    int QuestionNumber,
    string RightAnswer,
    string InputedAnswer)
{
    public int QuestionId { get; set; }
    public QuestionDto Question { get; set; } = null!;
    
    public int PartitionHistoryId { get; set; }
    [JsonIgnore] public PartitionHistoryDto PartitionHistory { get; set; } = null!;
};