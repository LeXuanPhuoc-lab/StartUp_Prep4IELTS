using System.ComponentModel.DataAnnotations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Tests;

public class CreateQuestionAnswerRequest
{
    [StringLength(maximumLength: 155, MinimumLength = 1, ErrorMessage = "Answer display must be between 1 and 155 characters long.")]
    [Required(ErrorMessage = "Answer display is required.")]
    public string AnswerDisplay { get; set; } = null!;

    [StringLength(maximumLength: 100, MinimumLength = 1, ErrorMessage = "Answer text must be between 1 and 100 characters long.")]
    [Required(ErrorMessage = "Answer text is required.")]
    public string AnswerText { get; set; } = null!;
    
    [Required(ErrorMessage = "Answer need to defined as is true or not.")]
    public bool IsTrue { get; set; }
}