using System.ComponentModel.DataAnnotations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Tests;

public class CreateQuestionRequest
{
    [StringLength(maximumLength: 255, MinimumLength = 5, ErrorMessage = "The test title must be between 5 and 255 characters long.")]
    public string? QuestionDesc { get; set; }
    
    public string? QuestionAnswerExplanation { get; set; }
    
    [Required(ErrorMessage = "Question number is required.")]
    public int QuestionNumber { get; set; }

    [Required(ErrorMessage = "Question need to be defined as multiple choice or not.")]
    public bool IsMultipleChoice { get; set; }

    [Required(ErrorMessage = "Answer(s) for question is required.")]
    public List<CreateQuestionAnswerRequest> QuestionAnswers { get; set; } = new();
}