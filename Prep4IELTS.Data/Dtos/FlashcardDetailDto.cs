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
    int FlashcardId,
    int? FlashcardDetailTagId)
{
    [JsonIgnore] public FlashcardDetailTagDto? FlashcardDetailTag { get; set; } = null!;
    [JsonIgnore] public FlashcardDto Flashcard = null!;
    [JsonIgnore] public ICollection<FlashcardExamGradeDto> FlashcardExamGrades { get; set; } = new List<FlashcardExamGradeDto>();
    [JsonIgnore]
    public virtual ICollection<VocabularyUnitScheduleDto> VocabularyUnitSchedules { get; set; } = new List<VocabularyUnitScheduleDto>();
    // [JsonIgnore] 
    // public ICollection<UserFlashcardProgressDto> UserFlashcardProgresses =
    //     new List<UserFlashcardProgressDto>();
}