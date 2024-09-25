using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IFlashcardDetailService
{
    Task<bool> InsertAsync(int flashcardId, FlashcardDetailDto flashcardDetailDto);
    Task<bool> InsertPrivacyAsync(int flashcardId, Guid userId, FlashcardDetailDto flashcardDetailDto);
    Task<bool> RemoveAsync(int flashcardDetailId);
    Task<bool> IsInPublicFlashcard(int flashcardDetailId);
    Task UpdateAsync(FlashcardDetailDto flashcardDetailDto);
    Task<FlashcardDetailDto?> FindByIdAsync(int flashcardDetailId);
}