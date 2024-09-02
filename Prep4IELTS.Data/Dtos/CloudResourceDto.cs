using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record CloudResourceDto(
    int CloudResourceId,
    string? PublicId,
    string Url,
    int? Bytes,
    DateTime CreateDate,
    DateTime? ModifiedDate)
{
    [JsonIgnore]
    public ICollection<TestSectionDto> TestSections { get; set; } = new List<TestSectionDto>();
    
    [JsonIgnore]
    public ICollection<TestSectionPartitionDto> TestSectionPartitions { get; set; } = new List<TestSectionPartitionDto>();
};