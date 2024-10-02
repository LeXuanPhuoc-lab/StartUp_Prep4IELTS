using System.ComponentModel.DataAnnotations;
using Prep4IELTS.Business.Validations;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace EXE202_Prep4IELTS.Payloads.Requests.Flashcards;

public class CreateFlashcardDetailRequest
{
    [Required(ErrorMessage = "Please enter vocabulary name")]
    public string WordText { get; set; } = null!;

    [Required(ErrorMessage = "Please enter description")]
    [MaxLength(300, ErrorMessage = "Definition maximum length is 300 characters")]
    public string Definition { get; set; } = null!;

    [Required(ErrorMessage = "Please select word form")]
    [MaxLength(100, ErrorMessage = "WordForm maximum length is 100 characters")]
    public string WordForm { get; set; } = null!;
    
    [MaxLength(100, ErrorMessage = "WordPronunciation maximum length is 100 characters")]
    public string? WordPronunciation { get; set; }

    [MaxLength(500, ErrorMessage = "Example maximum length is 500 characters")]
    public string? Example { get; set; }

    [MaxLength(200, ErrorMessage = "Description maximum length is 200 characters")]
    public string? Description { get; set; }

    [ImageFile]
    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "FlashcardId is required")]
    public int FlashcardId { get; set; }
}

public static class CreateFlashcardDetailRequestExtensions
{
    public static FlashcardDetailDto ToFlashcardDetailDto(this CreateFlashcardDetailRequest req, 
        string? publicId, string? imageUrl, int? fileBytes)
    {
        return new(
            FlashcardDetailId: 0,
            WordText: req.WordText,
            Definition: req.Definition,
            WordForm: req.WordForm,
            WordPronunciation: req.WordPronunciation,
            Example: req.Example,
            Description: req.Description,
            CloudResourceId: null,
            CloudResource: !string.IsNullOrEmpty(imageUrl) 
                ? new CloudResourceDto(
                    CloudResourceId: 0, 
                    PublicId: publicId, 
                    Url: imageUrl, 
                    Bytes: fileBytes, 
                    CreateDate: DateTime.UtcNow, null) 
                : null,
            FlashcardId: 0,
            FlashcardDetailTagId: null);
    }
}