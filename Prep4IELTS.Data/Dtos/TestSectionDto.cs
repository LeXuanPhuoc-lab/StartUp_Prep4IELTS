using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record TestSectionDto(
    int TestSectionId,
    string TestSectionName,
    string? ReadingDesc,
    string? AudioResourceUrl,
    int TotalQuestion,
    string? SectionTranscript,
    ICollection<TestSectionPartitionDto> TestSectionPartitions)
{
    public Guid TestId{ get; set; }
    [JsonIgnore] public TestDto Test { get; set; } = null!;
};