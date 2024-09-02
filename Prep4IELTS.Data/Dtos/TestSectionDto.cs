using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record TestSectionDto(
    int TestSectionId,
    string TestSectionName,
    string? ReadingDesc,
    int TotalQuestion,
    string? SectionTranscript,
    int? CloudResourceId, CloudResourceDto CloudResource,
    ICollection<TestSectionPartitionDto> TestSectionPartitions)
{
    public Guid TestId{ get; set; }
    [JsonIgnore] public TestDto Test { get; set; } = null!;
};