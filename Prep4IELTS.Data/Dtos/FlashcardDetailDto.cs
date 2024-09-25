using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record FlashcardDetailDto(
    int FlashcardDetailId,
    string WordText,
    string Definition,
    string WordForm,
    string? WordPronunciation,
    string? Example,
    string? Description,
    // string? ImageUrl,
    int? CloudResourceId,
    CloudResourceDto? CloudResource,
    int FlashcardId)
{
    [JsonIgnore] public FlashcardDto Flashcard = null!;

    // [JsonIgnore] 
    // public ICollection<UserFlashcardProgressDto> UserFlashcardProgresses =
    //     new List<UserFlashcardProgressDto>();
}