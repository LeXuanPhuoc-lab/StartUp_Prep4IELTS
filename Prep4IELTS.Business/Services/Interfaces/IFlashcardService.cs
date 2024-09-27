using System.Linq.Expressions;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IFlashcardService
{
    Task<bool> InsertAsync(FlashcardDto flashcard);
    Task<bool> InsertPrivacyAsync(FlashcardDto flashcard, Guid userId);
    Task<bool> AddFlashcardToUserAsync(int flashcardId, Guid userId);
    Task<bool> RemoveAsync(int flashcardId);
    Task<bool> RemoveUserFlashcardAsync(int flashcardId, Guid userId);
    Task UpdateUserFlashcardProgressStatusAsync(
        int userFlashcardProgressId, FlashcardProgressStatus status);
    Task UpdateAsync(FlashcardDto flashcard);
    Task<IList<FlashcardDto>> FindAllAsync();
    Task<IList<FlashcardDto>> FindAllWithConditionAsync(
        Expression<Func<Flashcard, bool>>? filter = null,
        Func<IQueryable<Flashcard>, IOrderedQueryable<Flashcard>>? orderBy = null,
        string? includeProperties = "");
    Task<IList<FlashcardDto>> FindAllPrivacyWithConditionAsync(
        Expression<Func<Flashcard, bool>>? filter,
        Func<IQueryable<Flashcard>, IOrderedQueryable<Flashcard>>? orderBy,
        string? includeProperties,
        Guid userId);
    Task<IList<FlashcardDto>> FindAllWithConditionAndPagingAsync(
        Expression<Func<Flashcard, bool>>? filter,
        Func<IQueryable<Flashcard>, IOrderedQueryable<Flashcard>>? orderBy,
        string? includeProperties,
        int? pageIndex, int? pageSize);
    Task<FlashcardDto?> FindByIdAsync(int flashcardId);
    Task<FlashcardDto?> FindByIdAsync(int flashcardId, Guid userId);
    Task<int> CountTotalAsync();
    Task<bool> PublishAsync(int flashcardId);
    Task<bool> HideAsync(int flashcardId);
    Task<bool> IsAlreadyPublishedAsync(int flashcardId);
    Task<bool> IsExistAsync(int flashcardId);
    Task<bool> IsPublicAsync(int flashcardId);
    Task<bool> IsExistUserFlashcardAsync(int flashcardId, Guid userId);
    Task<FlashcardProgressModel> CalculateAllUserFlashcardProgressAsync(Guid userId);

    Task<(int totalNew, int totalStudying, int totalProficient, int totalStarred)> CalculateUserFlashcardProgressAsync(
        int flashcardId, Guid userId);
}