using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Responses.Tests;

public class TestSectionPartitionAnalyticsResponse
{
    public string PartitionTagDesc { get; set; } = string.Empty;
    public int TotalRight { get; set; } = 0;
    public int TotalWrong { get; set; } = 0;
    public double AccuracyRate { get; set; } = 0;
}

public static class TestSectionPartitionAnalyticsResponseExtension
{
    public static TestSectionPartitionAnalyticsResponse
        AddDetail(this TestSectionPartitionAnalyticsResponse resp, 
            string partitionTagDesc,
            List<PartitionHistoryDto> partitionHistoryDtos)
    {
        resp.PartitionTagDesc = partitionTagDesc;
        resp.TotalRight = partitionHistoryDtos.Sum(ph => ph.TotalRightAnswer) ?? 0;
        resp.TotalWrong = partitionHistoryDtos.Sum(ph => ph.TotalWrongAnswer) ?? 0;
        resp.AccuracyRate = partitionHistoryDtos.Average(ph => ph.AccuracyRate) ?? 0;

        return resp;
    }
}