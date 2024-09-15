using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Responses.Tests;

public class TestCategoryAnalyticsResponse
{
    public string TestCategoryName { get; set; } = string.Empty;
    public TestAnalyticsResponse TestAnalytics { get; set; } = new();
}

public static class TestCategoryAnalyticsResponseExtension
{
    public static TestCategoryAnalyticsResponse AddTestAnalyticsAsync(
        this TestCategoryAnalyticsResponse resp,
        DateTime? testTakenDate,
        string targetScore,
        string testCategoryName,
        List<TestHistoryDto> testHistoryDtos)
    {
        // Assign test category name
        resp.TestCategoryName = testCategoryName;
        // Initiate test analytics
        resp.TestAnalytics = new()
        {
            TotalTestEngaged = testHistoryDtos.Count,
            TotalPracticeTime = testHistoryDtos.Sum(x => x.TotalCompletionTime),
            // Assign test taken date (can empty)
            TestTakenDate = testTakenDate,
            // Assign target score (can empty)
            TargetScore = targetScore
        };
        // Add test type analytics  
        resp.TestAnalytics.AddTestTypeAnalyticsResponse(testHistoryDtos);
        
        return resp;
    }
}