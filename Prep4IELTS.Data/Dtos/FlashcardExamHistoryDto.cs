using System.Text.Json.Serialization;
using Prep4IELTS.Data.Entities;

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
    ICollection<FlashcardExamGrade> FlashcardExamGrades)
{
    [JsonIgnore] public UserFlashcardDto UserFlashcard { get; set; } = null!;
};