using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Enum;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IUserFlashcardService
{
    Task<bool> InsertAsync(UserFlashcardDto userFlashcardDto);
    Task<UserFlashcardDto> FindByUserAndFlashcardIdAsync(int flashcardId, Guid userId);
    Task<UserFlashcardDto?> FindUserPracticingProgressAsync(int flashcardId, Guid userId, bool? isTrackProgress = false);
    Task ResetFlashcardProgressAsync(int flashcardId, Guid userId);
    Task<UserFlashcardDto?> FindUserPracticingProgressWithStatusAsync(
        int flashcardId, Guid userId, List<FlashcardProgressStatus> statuses);
}