using System.Linq.Expressions;
using Mapster;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;

namespace Prep4IELTS.Business.Services;

public class FlashcardService(
    UnitOfWork unitOfWork) : IFlashcardService
{
    public async Task<bool> InsertAsync(FlashcardDto flashcard)
    {
        await unitOfWork.FlashcardRepository.InsertAsync(flashcard.Adapt<Flashcard>());
        return await unitOfWork.FlashcardRepository.SaveChangeWithTransactionAsync() > 0;
    }
    public async Task<bool> InsertPrivacyAsync(FlashcardDto flashcard, Guid userId)
    {
        await unitOfWork.FlashcardRepository.InsertPrivacyAsync(flashcard.Adapt<Flashcard>(), userId);
        return await unitOfWork.FlashcardRepository.SaveChangeWithTransactionAsync() > 0;
    }
    public async Task<bool> AddFlashcardToUserAsync(int flashcardId, Guid userId)
    {
        await unitOfWork.FlashcardRepository.AddFlashcardToUserAsync(flashcardId, userId);
        return await unitOfWork.FlashcardRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> RemoveAsync(int flashcardId)
    {
        await unitOfWork.FlashcardRepository.RemoveAsync(flashcardId);
        return await unitOfWork.FlashcardRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> RemovePrivacyAsync(int flashcardId, Guid userId)
    {
        await unitOfWork.FlashcardRepository.RemoveUserFlashcardAsync(flashcardId, userId);
        return await unitOfWork.FlashcardRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task UpdateUserFlashcardProgressStatusAsync(int userFlashcardProgressId, FlashcardProgressStatus status)
    {
        await unitOfWork.FlashcardRepository.UpdateUserFlashcardProgressStatusAsync(
            userFlashcardProgressId, status);
        await unitOfWork.FlashcardRepository.SaveChangeWithTransactionAsync();
    }

    public async Task UpdateAsync(FlashcardDto flashcardDto)
    {
        // Get flashcard by id
        var flashcardEntity = 
            await unitOfWork.FlashcardRepository.FindOneWithConditionAsync(
                f => f.FlashcardId == flashcardDto.FlashcardId);

        if (flashcardEntity == null!) return;
        
        // Update flashcard properties
        flashcardEntity.Title = flashcardDto.Title;
        flashcardEntity.Description = flashcardDto.Description;
        
        await unitOfWork.FlashcardRepository.UpdateAsync(flashcardEntity);
        await unitOfWork.FlashcardRepository.SaveChangeWithTransactionAsync();
    }

    public async Task UpdateFlashcardTotalViewAsync(int flashcardId)
    {
        await unitOfWork.FlashcardRepository.UpdateFlashcardTotalViewAsync(flashcardId);
        await unitOfWork.FlashcardRepository.SaveChangeWithTransactionAsync();
    }

    public async Task<IList<FlashcardDto>> FindAllAsync()
    {
        var flashcardEntities = await unitOfWork.FlashcardRepository.FindAllAsync();
        return flashcardEntities.Adapt<List<FlashcardDto>>();
    }
    public async Task<IList<FlashcardDto>> FindAllWithConditionAsync(
        Expression<Func<Flashcard, bool>>? filter = null, 
        Func<IQueryable<Flashcard>, IOrderedQueryable<Flashcard>>? orderBy = null, 
        string? includeProperties = "")
    {
        var flashcardEntities = await unitOfWork.FlashcardRepository.FindAllWithConditionAsync(
            filter, orderBy, includeProperties);
        
        return flashcardEntities.Adapt<List<FlashcardDto>>();
    }

    public async Task<IList<FlashcardDto>> FindAllWithConditionForUserAsync(
        Expression<Func<Flashcard, bool>>? filter, 
        Func<IQueryable<Flashcard>, IOrderedQueryable<Flashcard>>? orderBy, 
        string? includeProperties, 
        Guid? userId)
    {
        var flashcardEntities = await unitOfWork.FlashcardRepository.FindAllWithConditionAsync(
            filter, orderBy, includeProperties);
        
        // Retrieve flashcard progresses
        if (userId != null && userId != Guid.Empty)
        {
            foreach (var fCard in flashcardEntities)
            {
                await unitOfWork.FlashcardRepository.FindUserFlashcardAsync(fCard.FlashcardId, userId.Value);
            }
        }
        
        return flashcardEntities.Adapt<List<FlashcardDto>>();
    }

    public async Task<IList<FlashcardDto>> FindAllWithConditionAndPagingAsync(
        Expression<Func<Flashcard, bool>>? filter, 
        Func<IQueryable<Flashcard>, IOrderedQueryable<Flashcard>>? orderBy, 
        string? includeProperties, int? pageIndex,
        int? pageSize)
    {
        var flashcardEntities = await unitOfWork.FlashcardRepository.FindAllWithConditionAndPagingAsync(
            filter, orderBy, includeProperties, pageIndex, pageSize);
        return flashcardEntities.Adapt<List<FlashcardDto>>();
    }
    public async Task<FlashcardDto?> FindByIdAsync(int flashcardId)
    {
        var flashcardEntity = await unitOfWork.FlashcardRepository.FindByIdAsync(flashcardId);
        return flashcardEntity.Adapt<FlashcardDto>();
    }
    public async Task<FlashcardDto?> FindByIdAsync(int flashcardId, Guid userId)
    {
        var flashcardEntity = await unitOfWork.FlashcardRepository.FindByIdAsync(flashcardId, userId);
        return flashcardEntity.Adapt<FlashcardDto>();
    }

    public async Task<int> CountTotalAsync()
    {
        return await unitOfWork.FlashcardRepository.CountTotalAsync();
    }

    public async Task<bool> PublishAsync(int flashcardId)
    {
        return await unitOfWork.FlashcardRepository.PublishAsync(flashcardId);
    }

    public async Task<bool> HideAsync(int flashcardId)
    {
        return await unitOfWork.FlashcardRepository.HideAsync(flashcardId);
    }

    public async Task<bool> IsAlreadyPublishedAsync(int flashcardId)
    {
        return await unitOfWork.FlashcardRepository.IsAlreadyPublishedAsync(flashcardId);
    }
    public async Task<bool> IsExistAsync(int flashcardId)
    {
        return await unitOfWork.FlashcardRepository.IsExistAsync(flashcardId);
    }
    public async Task<bool> IsPublicAsync(int flashcardId)
    {
        return await unitOfWork.FlashcardRepository.IsPublicAsync(flashcardId);
    }
    public async Task<bool> IsExistUserFlashcardAsync(int flashcardId, Guid userId)
    {
        return await unitOfWork.FlashcardRepository.IsExistUserFlashcardAsync(
            flashcardId, userId);
    }

    public async Task<FlashcardProgressModel> CalculateAllUserFlashcardProgressAsync(Guid userId)
    {
        var flashcardProgress = 
            await unitOfWork.FlashcardRepository.CalculateAllUserFlashcardProgressAsync(userId);

        return new()
        {
            TotalNew = flashcardProgress.totalNew,
            TotalStudying = flashcardProgress.totalStudying,
            TotalProficient = flashcardProgress.totalProficient,
            TotalStarred = flashcardProgress.totalStarred
        };
    }

    public async Task<(int totalNew, int totalStudying, int totalProficient, int totalStarred)> 
        CalculateUserFlashcardProgressAsync(int flashcardId, Guid userId)
    {
        return await unitOfWork.FlashcardRepository.CalculateUserFlashcardProgressAsync(flashcardId, userId);
    }
}