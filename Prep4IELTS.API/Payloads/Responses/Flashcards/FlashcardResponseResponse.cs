using Prep4IELTS.Business.Constants;
using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Responses.Flashcards;

public class FlashcardResponseResponse
{
    public int FlashcardId { get; set; }

    public string Title { get; set; } = null!;

    public int? TotalWords { get; set; }

    public int? TotalView { get; set; }

    public string? Description { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool IsPublic { get; set; }
    public List<UserFlashcardResponse> UserFlashcards { get; set; } = new ();
    public List<FlashcardDetailDto> NewFlashCardDetails { get; set; } = new ();
    public List<FlashcardDetailDto> StudyingFlashCardDetails { get; set; } = new ();
    public List<FlashcardDetailDto> ProficientFlashCardDetails { get; set; } = new ();
    public List<FlashcardDetailDto> StarredFlashCardDetails { get; set; } = new ();
}

public static class FlashcardResponseExtensions
{
    public static async Task<FlashcardResponseResponse> ToPrivacyFlashcardResponseAsync(
        this FlashcardDto flashcardDto, 
        List<UserFlashcardProgressDto> userFlashcardProgress)
    {
        var flashcardForUserResponse = new FlashcardResponseResponse()
        {
            FlashcardId = flashcardDto.FlashcardId,
            Title = flashcardDto.Title,
            TotalView = flashcardDto.TotalView,
            TotalWords = flashcardDto.TotalWords,
            Description = flashcardDto.Description,
            CreateDate = flashcardDto.CreateDate,
            IsPublic = flashcardDto.IsPublic
        };
        
        foreach (var flp in userFlashcardProgress)
        {
            switch (flp.ProgressStatus)
            {
                case FlashcardProgressConstants.New:
                    flashcardForUserResponse.NewFlashCardDetails.Add(flp.FlashcardDetail);
                    break;
                case FlashcardProgressConstants.Studying:
                    flashcardForUserResponse.StudyingFlashCardDetails.Add(flp.FlashcardDetail);
                    break;
                case FlashcardProgressConstants.Proficient:
                    flashcardForUserResponse.ProficientFlashCardDetails.Add(flp.FlashcardDetail);
                    break;
                case FlashcardProgressConstants.Starred:
                    flashcardForUserResponse.StarredFlashCardDetails.Add(flp.FlashcardDetail);
                    break;
            }
        }
        
        return await Task.FromResult(flashcardForUserResponse);
    }

    public static async Task<FlashcardResponseResponse> ToPrivacyFlashcardResponseAsync(
        this FlashcardDto flashcardDto, int totalNew, int totalStudying, int totalProficient, int totalStarred)
    {
        var flashcardForUserResponse = new FlashcardResponseResponse()
        {
            FlashcardId = flashcardDto.FlashcardId,
            Title = flashcardDto.Title,
            TotalView = flashcardDto.TotalView,
            TotalWords = flashcardDto.TotalWords,
            Description = flashcardDto.Description,
            CreateDate = flashcardDto.CreateDate,
            IsPublic = flashcardDto.IsPublic
        };

        if (flashcardDto.UserFlashcards != null! 
            && flashcardDto.UserFlashcards.Any())
        {
            var firstUserFlashcard = flashcardDto.UserFlashcards.First();
            
            flashcardForUserResponse.UserFlashcards.Add(new ()
            {
                UserFlashcardId = firstUserFlashcard.UserFlashcardId,
                FlashcardId = flashcardDto.FlashcardId,
                UserId = firstUserFlashcard.UserId,
                User = firstUserFlashcard.User,
                TotalNew = totalNew,
                TotalStudying = totalStudying,
                TotalProficient = totalProficient,
                TotalStarred = totalStarred
            });
        }
        
        return await Task.FromResult(flashcardForUserResponse);
    }
}