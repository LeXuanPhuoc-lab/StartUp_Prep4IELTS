using System.ComponentModel.DataAnnotations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Flashcards;

public class UpdatePrivacyFlashcardRequest
{
    [Required(ErrorMessage = "Flashcard title is required.")]
    public string Title { get; set; } = String.Empty;

    [MaxLength(length: 200, ErrorMessage = "Flashcard description must be less than 200 characters")]
    public string? Description { get; set; } = String.Empty;
}
