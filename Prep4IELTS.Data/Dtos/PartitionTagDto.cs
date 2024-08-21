using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record PartitionTagDto(int PartitionTagId, string? PartitionTagDesc)
{
    [JsonIgnore]
    public ICollection<TestSectionPartitionDto> TestSectionPartitions { get; set; } 
        = new List<TestSectionPartitionDto>();
};