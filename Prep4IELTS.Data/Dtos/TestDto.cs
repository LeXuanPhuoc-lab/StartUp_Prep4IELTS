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
    string? CreateBy,
    DateTime CreateDate, DateTime? ModifiedDate,
    int TestCategoryId, TestCategoryDto TestCategory,
    ICollection<TagDto> Tags,
    ICollection<CommentDto> Comments,
    ICollection<TestHistoryDto> TestHistories,
    ICollection<TestSectionDto> TestSections);