using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record FlashcardDetail(
    int FlashcardDetailId,
    string WordText,
    string Definition,
    string WordForm,
    string? WordPronunciation,
    string? Example,
    string? Description,
    string? ImageUrl)
{
    public int FlashcardId{ get; set; }
    [JsonIgnore] public FlashcardDto Flashcard = null!;
};