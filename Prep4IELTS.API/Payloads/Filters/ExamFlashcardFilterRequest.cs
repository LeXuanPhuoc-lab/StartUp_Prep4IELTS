using System.ComponentModel.DataAnnotations;
using Prep4IELTS.Business.Constants;

namespace EXE202_Prep4IELTS.Payloads.Filters;

public class ExamFlashcardFilterRequest
{
    [Required(ErrorMessage = "Total question is required")]
    public int TotalQuestion { get; set; } = 20;

    [Required(ErrorMessage = "Please identify question formation")]
    public bool IsTermPattern { get; set; } = false;
    public List<string> QuestionTypes { get; set; } = new();
}