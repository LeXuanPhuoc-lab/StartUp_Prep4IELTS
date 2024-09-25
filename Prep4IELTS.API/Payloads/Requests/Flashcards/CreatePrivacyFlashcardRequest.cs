using System.ComponentModel.DataAnnotations;
using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Requests.Flashcards;

public class CreatePrivacyFlashcardRequest
{
    [Required(ErrorMessage = "You must provide a name for the new flashcard")]
    [StringLength(maximumLength: 100, MinimumLength = 1, ErrorMessage = "Flashcard name must be between 1 and 100 characters")]
    public string Title { get; set; } = string.Empty;
    
    
    [MaxLength(length: 200, ErrorMessage = "Flashcard description must be less than 200 characters")]
    public string? Description { get; set; } = null!; 
}

public static class CreatePrivacyFlashcardRequestExtensions
{
    public static FlashcardDto ToFlashcardDto(this CreatePrivacyFlashcardRequest req)
    {
        return new FlashcardDto(
            FlashcardId:0, 
            Title: req.Title, 
            TotalWords: 0,
            TotalView: 0, 
            Description: req.Description,
            CreateDate: DateTime.UtcNow, 
            IsPublic: false, null!, null!);   
    }
}