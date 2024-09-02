using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Responses.Tests;

public class TestHistoryResponse
{
    public TestHistoryDto TestHistory { get; set; } = null!;
    // public SectionHistoryResponse SectionHistoryResp { get; set; } = null!;
    public List<SectionHistoryResponse> SectionHistories { get; set; } = null!;
}