using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record SpeakingPartDto(
    int SpeakingPartId,
    int SpeakingPartNumber,
    string? SpeakingPartDescription,
    bool IsActive,
    DateTime CreateDate)
{
    public int SpeakingSampleId { get; set; }
    [JsonIgnore] public SpeakingSampleDto SpeakingSample { get; set; } = null!;
};