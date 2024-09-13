using System.ComponentModel.DataAnnotations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Tests;

public class UpdateTestGradeRequest
{
    // [Required(ErrorMessage = "TestGradeId is required.")]
    // public int TestGradeId { get; set; }
    
    [Required(ErrorMessage = "QuestionId is required.")]
    public int QuestionId { get; set; }
    
    [Required(ErrorMessage = "InputedAnswer is required.")]
    public string InputedAnswer { get; set; } = null!;
}