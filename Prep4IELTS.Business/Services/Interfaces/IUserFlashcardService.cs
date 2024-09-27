using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Enum;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IUserFlashcardService
{
    Task<bool> InsertAsync(UserFlashcardDto userFlashcardDto);
    Task<UserFlashcardDto?> GetUserPracticingProgressAsync(int flashcardId, Guid userId);
    Task ResetFlashcardProgressAsync(int flashcardId, Guid userId);
    Task<UserFlashcardDto?> GetUserPracticingProgressWithStatusAsync(
        int flashcardId, Guid userId, List<FlashcardProgressStatus> statuses);
}