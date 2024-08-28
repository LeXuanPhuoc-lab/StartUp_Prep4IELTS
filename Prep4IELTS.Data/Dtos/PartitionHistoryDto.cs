using System.Text.Json.Serialization;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Dtos;

public record PartitionHistoryDto(
    int PartitionHistoryId,
    string TestSectionName,
    int? TotalRightAnswer,
    int? TotalWrongAnswer,
    int? TotalSkipAnswer,
    double? AccuracyRate,
    int TotalQuestion,
    int TestHistoryId,
    ICollection<TestGradeDto> TestGrades,
    int TestSectionPartId,
    TestSectionPartitionDto TestSectionPart)
{
    [JsonIgnore] public TestHistory TestHistory { get; set; } = null!;
};