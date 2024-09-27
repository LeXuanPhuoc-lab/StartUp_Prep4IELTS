using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record UserFlashcardProgressDto(
    int UserFlashcardProgressId,
    string ProgressStatus,
    int FlashcardDetailId,
    FlashcardDetailDto FlashcardDetail)
{
    public int UserFlashcardId { get; set; }
    [JsonIgnore]
    public UserFlashcardDto UserFlashcard { get; set; } = null!;
};