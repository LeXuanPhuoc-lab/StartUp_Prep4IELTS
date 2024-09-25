using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record FlashcardDto(
    int FlashcardId,
    string Title,
    int? TotalWords,
    int? TotalView,
    string? Description,
    DateTime? CreateDate,
    bool IsPublic,
    ICollection<FlashcardDetailDto> FlashcardDetails,
    ICollection<UserFlashcardDto> UserFlashcards);