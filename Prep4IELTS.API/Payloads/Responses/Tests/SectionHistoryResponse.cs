using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Responses.Tests;

public class SectionHistoryResponse
{
    public int TestSectionId { get; set; }
    public string SectionName { get; set; } = string.Empty;
    public int? TotalRightAnswer { get; set; }
    public int? TotalWrongAnswer { get; set; }
    public int? TotalSkipAnswer { get; set; }
    public double? AccuracyRate { get; set; }
    
    public CloudResourceDto CloudResource { get; set; } = null!;
    
    // public PartitionHistoryDetailResponse PartitionHistoryResp { get; set; } = null!;
    public List<PartitionHistoryDto> PartitionHistories { get; set; } = new ();
}

public static class PartitionHistoryResponseExtension
{
    public static SectionHistoryResponse CalculateTotal(this SectionHistoryResponse resp, 
        string sectionName, int totalQuestion, List<PartitionHistoryDto> partitionHistories)
    {
        SectionHistoryResponse sectionHistoryResponse = new()
        {
            SectionName = sectionName,
            TotalRightAnswer = partitionHistories.Sum(x => x.TotalRightAnswer),
            TotalSkipAnswer = partitionHistories.Sum(x => x.TotalSkipAnswer),
            TotalWrongAnswer = partitionHistories.Sum(x => x.TotalWrongAnswer),
            PartitionHistories = partitionHistories
        };
        // Update accuracy rate
        var totalRightAnswer = sectionHistoryResponse.TotalRightAnswer;
        var totalPartitionQuestion = partitionHistories.SelectMany(x => x.TestGrades).Count();
        sectionHistoryResponse.AccuracyRate = (totalRightAnswer < 1) 
            ? 0 
            : totalRightAnswer / (double)totalPartitionQuestion;

        return sectionHistoryResponse;
    }
}