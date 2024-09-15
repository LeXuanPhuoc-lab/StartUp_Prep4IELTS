using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Responses.Tests;

public class TestSectionAnalyticsResponse
{
    public string SectionName { get; set; } = string.Empty;
    public List<TestSectionPartitionAnalyticsResponse> TestSectionPartitionAnalytics { get; set; } = new();
}

public static class TestSectionAnalyticsResponseExtension
{
    public static TestSectionAnalyticsResponse AddTestSectionPartitionAnalytics(
        this TestSectionAnalyticsResponse resp,
        string sectionName,
        List<PartitionHistoryDto> partitionHistoryDtos)
    {
        // Assign test section name 
        resp.SectionName = sectionName;
        // Group partition history by partition tag
        resp.TestSectionPartitionAnalytics = partitionHistoryDtos
            .GroupBy(g => g.TestSectionPart.PartitionTag.PartitionTagDesc)
            .Select(g => 
                new TestSectionPartitionAnalyticsResponse().AddDetail(
                    partitionTagDesc: g.Key ?? string.Empty, 
                    partitionHistoryDtos: g.Select(ph => ph).ToList()))
            .ToList();

        return resp;
    }
}