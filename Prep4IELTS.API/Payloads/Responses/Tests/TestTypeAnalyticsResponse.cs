using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Responses.Tests;

public class TestTypeAnalyticsResponse
{
    public string TestType { get; set; } = string.Empty;
    public int TotalTestEngaged { get; set; }
    public double? AverageAccuracyRate { get; set; }
    public double? AveragePracticeTime { get; set; }
    public string AverageScore { get; set; } = string.Empty;
    public string HighestScore { get; set; } = string.Empty;
    public List<TestSectionAnalyticsResponse> SectionsAnalytics { get; set; } = new();
}

public static class TestTypeAnalyticsResponseExtension
{
    public static TestTypeAnalyticsResponse AddSectionsAnalytics(
        this TestTypeAnalyticsResponse resp,
        string testType,
        List<TestHistoryDto> testHistoryDtos)
    {
        // Assign test type for resp
        resp.TestType = testType;   
        // Analyze result
        resp.TotalTestEngaged = testHistoryDtos.Count;
        resp.AverageAccuracyRate = testHistoryDtos.Select(x => x.AccuracyRate).Average();
        resp.AveragePracticeTime = testHistoryDtos.Select(x => x.TotalCompletionTime).Average();
        var bandScoreList = testHistoryDtos
            .Select(x => double.Parse(x.BandScore ?? "0"));
        resp.HighestScore = bandScoreList.Max().ToString("F1");
        
        // Calculate the average band score 
        var averageBandScore = testHistoryDtos
            .Where(x => x.BandScore != null)
            .Select(x => double.Parse(x.BandScore ?? "0"))
            .Average();
        // Apply average calculation logic
        var averageCalResult = (averageBandScore % 1 >= 0.75)
            ? Math.Ceiling(averageBandScore)
            : (averageBandScore % 1 >= 0.25)
                ? Math.Floor(averageBandScore) + 0.5
                : Math.Floor(averageBandScore);
        // Take or establish 1 digit after dot
        resp.AverageScore = averageCalResult.ToString("F1");    
        
        
        // Partition histories
        var partitionHistories = testHistoryDtos.SelectMany(x => x.PartitionHistories).ToList();
        
        // Group partition history by test section
        resp.SectionsAnalytics = partitionHistories
            .GroupBy(g => g.TestSectionName)
            .Select(g => new TestSectionAnalyticsResponse().AddTestSectionPartitionAnalytics(
                sectionName: g.Key,
                partitionHistoryDtos: g.Select(ph => ph).ToList())).ToList();
        
        return resp;
    }
}