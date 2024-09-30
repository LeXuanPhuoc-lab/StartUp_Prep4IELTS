using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IFlashcardExamHistoryService
{
    Task<bool> InsertAsync(
        FlashcardExamHistoryDto flashcardExamHistoryDto, bool isTermPattern);
}