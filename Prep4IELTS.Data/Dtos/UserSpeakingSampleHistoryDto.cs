using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record UserSpeakingSampleHistoryDto(
    int UserSampleHistoryId,
    Guid UserId,
    UserDto User)
{
    public int SpeakingSampleId { get; set; }
    [JsonIgnore] public SpeakingSampleDto SpeakingSample { get; set; } = null!;
};