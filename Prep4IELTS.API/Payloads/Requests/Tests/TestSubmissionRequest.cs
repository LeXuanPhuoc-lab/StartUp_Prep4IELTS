using System.ComponentModel.DataAnnotations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Tests;

public class TestSubmissionRequest
{
    // Test History Information
    [Required]
    public Guid UserId { get; set; } 
    
    [Required]
    public int TestId { get; set; } 
    
    [Required]
    public int TotalCompletionTime { get; set; }
    
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime TakenDatetime { get; set; }
   
    [Required]
    public bool IsFull { get; set; }
    
    // Partition History Information
    
    // Question Answers
    [Required]
    public List<QuestionAnswerSubmissionRequest> QuestionAnswers { get; set; } = new();
}