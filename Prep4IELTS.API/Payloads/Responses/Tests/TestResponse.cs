using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Responses.Tests;

public class TestResponse
{
    public int Id { get; set; }

    public Guid TestId { get; set; }

    public string TestTitle { get; set; } = null!;

    public int Duration { get; set; }
    public string TestType { get; set; } = null!;

    public int? TotalEngaged { get; set; }

    public int TotalQuestion { get; set; }

    public int? TotalSection { get; set; }

    public int TestCategoryId { get; set; }

    public bool IsDraft { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
    
    public List<TestHistoryDto> TestHistories { get; set; } = new ();
}

public static class TestResponseExtensions
{
    public static List<TestResponse> ToListTestResponse(this List<TestHistoryDto> testHistories)
    {
        // Initiate list of test history response 
        return testHistories
            .GroupBy(testHistory => testHistory.Test.TestTitle)
            .Select(g => 
            {
                // Assuming that other properties in the group are the same for all entries
                var firstTest = g.First().Test; 
                return new TestResponse()
                {
                    TestTitle = g.Key,
                    TestId = firstTest.TestId,                
                    Duration = firstTest.Duration,           
                    TestType = firstTest.TestType,           
                    TotalEngaged = firstTest.TotalEngaged,  
                    TotalQuestion = firstTest.TotalQuestion, 
                    TotalSection = firstTest.TotalSection,   
                    TestCategoryId = firstTest.TestCategoryId,
                    IsDraft = firstTest.IsDraft,
                    UserId = firstTest.UserId,
                    CreateDate = firstTest.CreateDate,
                    ModifiedDate = firstTest.ModifiedDate,
                    TestHistories = g.ToList()               
                };
            }).ToList();
    }
}