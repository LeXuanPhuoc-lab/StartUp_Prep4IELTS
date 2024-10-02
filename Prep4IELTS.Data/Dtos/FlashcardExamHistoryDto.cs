using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record FlashcardExamHistoryDto(
    int FlashcardExamHistoryId,
    int? TotalQuestion,
    int? TotalRightAnswer,
    int? TotalWrongAnswer,
    int? TotalCompletionTime,
    double? AccuracyRate,
    DateTime TakenDate,
    int UserFlashcardId,
    bool IsTermPattern,
    ICollection<FlashcardExamGradeDto> FlashcardExamGrades)
{
    [JsonIgnore] public UserFlashcardDto UserFlashcard { get; set; } = null!;
};