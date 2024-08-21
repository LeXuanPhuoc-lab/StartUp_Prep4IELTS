using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record TagDto(int TagId, string? TagName)
{
    [JsonIgnore]
    public ICollection<TestDto> Tests { get; set; } = new List<TestDto>();
};