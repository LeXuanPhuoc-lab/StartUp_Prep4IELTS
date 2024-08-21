using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record TestDto(
    int Id,
    Guid TestId,
    string TestTitle,
    int Duration,
    string TestType,
    int? TotalEngaged,
    int TotalQuestion,
    int? TotalSection,
    int TestCategoryId, TestCategoryDto TestCategory,
    ICollection<TagDto> Tags
    )
{
    [JsonIgnore] public ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();
    [JsonIgnore] public ICollection<TestHistoryDto> TestHistories { get; set; } = new List<TestHistoryDto>();
    [JsonIgnore] public ICollection<TestSectionDto> TestSections { get; set; } = new List<TestSectionDto>();
};