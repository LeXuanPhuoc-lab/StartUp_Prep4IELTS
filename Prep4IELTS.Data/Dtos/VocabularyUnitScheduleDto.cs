using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record VocabularyUnitScheduleDto(
    int VocabularyUnitScheduleId,
    string? Weekday,
    string? Comment,
    DateTime CreateDate,
    int FlashcardDetailId,
    FlashcardDetailDto FlashcardDetail);