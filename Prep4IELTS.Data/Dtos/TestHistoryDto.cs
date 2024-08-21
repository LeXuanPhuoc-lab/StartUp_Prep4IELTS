using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Dtos;

public record TestHistoryDto(
    int TestHistoryId, 
    int? TotalRightAnswer, 
    int? TotalWrongAnswer, 
    int? TotalSkipAnswer, 
    int TotalQuestion, 
    int TotalCompletionTime, 
    DateTime TakenDate, 
    double? AccuracyRate, 
    bool IsFull, 
    string TestType, 
    string? BandScore, 
    Guid UserId, UserDto User,
    Guid TestId, TestDto Test, 
    int TestCategoryId, TestCategoryDto TestCategory,
    ICollection<PartitionHistoryDto> PartitionHistories);