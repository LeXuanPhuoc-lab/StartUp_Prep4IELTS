using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record UserFlashcardDto(
    int UserFlashcardId,
    ICollection<UserFlashcardProgressDto> UserFlashcardProgresses)
{
    public Guid UserId { get; set; }
    public UserDto User { get; set; } = null!;
    
    public int FlashcardId { get; set; }
    [JsonIgnore] public FlashcardDto Flashcard { get; set; } = null!;
}