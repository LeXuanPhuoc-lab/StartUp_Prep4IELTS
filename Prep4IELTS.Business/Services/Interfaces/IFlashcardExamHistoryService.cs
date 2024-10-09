using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using System.Linq.Expressions;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IFlashcardExamHistoryService
{
    Task<bool> InsertAsync(
        FlashcardExamHistoryDto flashcardExamHistoryDto, int userFlashcardId,
        bool isTermPattern, bool? isSaveWrongToVocabSchedule = false);

    Task<FlashcardExamHistoryDto?> FindByUserFlashcardIdAtTakenDateAsync(
        int userFlashcardId, DateTime takenDateTime);

    Task<List<FlashcardExamHistoryDto>> FindAllByUserAsync(
        Expression<Func<FlashcardExamHistory, bool>>? filter,
        Func<IQueryable<FlashcardExamHistory>, IOrderedQueryable<FlashcardExamHistory>>? orderBy,
        Guid userId);
}