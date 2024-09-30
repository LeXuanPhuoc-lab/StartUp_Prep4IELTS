using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;

namespace Prep4IELTS.Business.Services;

public class UserFlashcardService(UnitOfWork unitOfWork) : IUserFlashcardService
{
    public async Task<bool> InsertAsync(UserFlashcardDto userFlashcardDto)
    {
        await unitOfWork.UserFlashcardRepository.InsertAsync(userFlashcardDto.Adapt<UserFlashcard>());
        return await unitOfWork.UserFlashcardRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<UserFlashcardDto> FindByUserAndFlashcardIdAsync(int flashcardId, Guid userId)
    {
        var userFlashcardEntity = 
            await unitOfWork.UserFlashcardRepository.FindByUserAndFlashcardIdAsync(flashcardId, userId);
        return userFlashcardEntity.Adapt<UserFlashcardDto>();
    }

    public async Task<UserFlashcardDto?> FindUserPracticingProgressAsync(int flashcardId, Guid userId, bool? isTrackProgress = false)
    {
        var userFlashcardEntity = 
            await unitOfWork.UserFlashcardRepository.FindUserPracticingProgressAsync(flashcardId,userId, isTrackProgress);
        return userFlashcardEntity.Adapt<UserFlashcardDto>();
    }

    public async Task ResetFlashcardProgressAsync(int flashcardId, Guid userId)
    {
        await unitOfWork.UserFlashcardRepository.ResetFlashcardProgressAsync(flashcardId, userId);
        await unitOfWork.UserFlashcardRepository.SaveChangeWithTransactionAsync();
    }

    public async Task<UserFlashcardDto?> FindUserPracticingProgressWithStatusAsync(int flashcardId, Guid userId, 
        List<FlashcardProgressStatus> statuses)
    {
        var userFlashcardEntity = 
            await unitOfWork.UserFlashcardRepository.FindUserPracticingProgressWithStatusAsync(
                flashcardId,userId, statuses);
        return userFlashcardEntity.Adapt<UserFlashcardDto>();
    }
}