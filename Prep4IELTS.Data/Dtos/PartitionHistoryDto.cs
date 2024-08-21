using System.Text.Json.Serialization;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Dtos;

public record PartitionHistoryDto(
    int PartitionHistoryId,
    string TestSectionName,
    int? TotalRightAnswer,
    int? TotalWrongAnswer,
    int? TotalSkipAnswer,
    int TotalQuestion,
    ICollection<TestGrade> TestGrades)
{
    public int TestHistoryId { get; set; }
    [JsonIgnore] public TestHistory TestHistory { get; set; } = null!;
    
    public int TestSectionPartId { get; set; }
    [JsonIgnore] public TestSectionPartitionDto TestSectionPart { get; set; } = null!;
};