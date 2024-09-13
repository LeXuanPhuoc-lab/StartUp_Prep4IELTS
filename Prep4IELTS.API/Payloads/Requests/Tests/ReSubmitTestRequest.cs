using System.ComponentModel.DataAnnotations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Tests;

public class ReSubmitTestRequest
{
    [Required(ErrorMessage = "TestHistoryId is required.")]
    public int TestHistoryId { get; set; }
    
    [Required(ErrorMessage = "TotalCompletionTime is required.")]
    public int TotalCompletionTime { get; set; }
    
    [Required(ErrorMessage = "TakenDatetime is required.")]
    [DataType(DataType.DateTime)]
    public DateTime TakenDatetime { get; set; }

    [Required(ErrorMessage = "TestGrades is required.")]
    public List<UpdateTestGradeRequest> TestGrades { get; set; } = new();
}