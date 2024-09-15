using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Enum;

namespace EXE202_Prep4IELTS.Payloads.Responses.Tests;

public class TestAnalyticsResponse
{
    public int TotalTestEngaged { get; set; }
    public int TotalPracticeTime { get; set; }
    public DateTime? TestTakenDate { get; set; }
    public string? TargetScore { get; set; } = string.Empty;
    public List<TestTypeAnalyticsResponse> TestTypeAnalytics { get; set; } = new();
}

public static class TestAnalyticsResponseExtension
{
    public static TestAnalyticsResponse AddTestTypeAnalyticsResponse(
        this TestAnalyticsResponse testAnalyticsResponse, 
        List<TestHistoryDto> testHistoryDtos)
    {
        // Group test history by test type
        testAnalyticsResponse.TestTypeAnalytics = testHistoryDtos
            .GroupBy(g => g.TestType)
            .Select(g => new TestTypeAnalyticsResponse().AddSectionsAnalytics(
                testType: g.Key,
                testHistoryDtos: g.Select(th => th).ToList())).ToList();
        return testAnalyticsResponse;
    }
}