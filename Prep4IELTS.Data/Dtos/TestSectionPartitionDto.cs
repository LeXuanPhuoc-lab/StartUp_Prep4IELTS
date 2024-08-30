using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record TestSectionPartitionDto(
    int TestSectionPartId,
    string? PartitionDesc,
    bool IsVerticalLayout,
    string? PartitionImage,
    int PartitionTagId, PartitionTagDto PartitionTag,
    ICollection<QuestionDto> Questions)
{
    public int TestSectionId { get; set; }
    [JsonIgnore]
    public TestSectionDto TestSection { get; set; } = null!;
    
    [JsonIgnore]
    public ICollection<PartitionHistoryDto> PartitionHistories { get; set; } = new List<PartitionHistoryDto>();
};